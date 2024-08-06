using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.RecorridoDtos;
using TurismoApp.Common.Enums;
using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

public class RecorridoApplication : IRecorridoApplication
{
    private readonly IRecorridoRepository _recorridoRepository;
    private readonly ICiudadRepository _ciudadRepository;
    private readonly IMapper _mapper;
    private readonly IClienteRepository _clienteRepository;
    private readonly AppDbContext _context;

    public RecorridoApplication(IRecorridoRepository recorridoRepository, ICiudadRepository ciudadRepository, IClienteRepository clienteRepository, IMapper mapper, AppDbContext context)
    {
        _recorridoRepository = recorridoRepository;
        _ciudadRepository = ciudadRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<RecorridoDto>> GetAllAsync()
    {
        var recorridos = await _context.Recorridos
        .Include(r => r.CiudadOrigen)
        .Include(r => r.CiudadDestino)
        .Include(r => r.ClienteRecorridos)
            .ThenInclude(cr => cr.Cliente)
        .ToListAsync();

        return _mapper.Map<IEnumerable<RecorridoDto>>(recorridos);
    }

    public async Task<RecorridoDto> GetByIdAsync(int id)
    {
        var recorrido = await _recorridoRepository.GetByIdAsync(id);
        if (recorrido == null)
        {
            return null;
        }
        return _mapper.Map<RecorridoDto>(recorrido);
    }

    public async Task<ResponseDto> AddAsync(CreateRecorridoDto createRecorridoDto)
    {
        if (await ExisteRecorridoConMismasCiudadesYFecha(createRecorridoDto.CiudadOrigenId, createRecorridoDto.CiudadDestinoId, createRecorridoDto.FechaViaje))
        {
            return ResponseDto.Error("Ya existe un recorrido con las mismas ciudades de origen y destino en la misma fecha.");
        }

        var recorrido = _mapper.Map<Recorrido>(createRecorridoDto);
        recorrido.Codigo = await GenerateCodigo(createRecorridoDto.CiudadOrigenId, createRecorridoDto.CiudadDestinoId, createRecorridoDto.FechaViaje);
        recorrido.Distancia = await CalculateDistance(createRecorridoDto.CiudadOrigenId, createRecorridoDto.CiudadDestinoId);
        recorrido.Precio = CalculatePrice(recorrido.Distancia, createRecorridoDto.FechaViaje);
        recorrido.Estado = EstadoRecorrido.Pendiente;
        recorrido.ClienteRecorridos = new List<ClienteRecorrido>();

        if (createRecorridoDto.Pasajeros != null && createRecorridoDto.Pasajeros.Any())
        {
            foreach (var clienteId in createRecorridoDto.Pasajeros)
            {
                var clienteDto = await _clienteRepository.GetClienteByIdAsync(clienteId);
                if (clienteDto == null)
                {
                    return ResponseDto.Error($"Cliente con ID {clienteId} no encontrado.");
                }

                var cliente = _mapper.Map<Cliente>(clienteDto);

                var existingCliente = await _context.Clientes.FindAsync(cliente.Id);
                if (existingCliente != null)
                {
                    _context.Entry(existingCliente).State = EntityState.Detached;
                }

                _context.Attach(cliente);

                recorrido.ClienteRecorridos.Add(new ClienteRecorrido { ClienteId = clienteId, Cliente = cliente });
            }
        }

        await _recorridoRepository.AddAsync(recorrido);
        var recorridoDto = _mapper.Map<RecorridoDto>(recorrido);
        return ResponseDto.Ok(recorridoDto);
    }

    public async Task<ResponseDto> UpdateAsync(int id, UpdateRecorridoDto updateRecorridoDto)
    {
        var recorrido = await _recorridoRepository.GetByIdAsync(id);
        if (recorrido == null)
        {
            return ResponseDto.Error("Recorrido no encontrado.");
        }

        if (recorrido.Estado != EstadoRecorrido.Pendiente)
        {
            return ResponseDto.Error("Sólo se pueden modificar recorridos en estado 'Pendiente'.");
        }

        if (await ExisteRecorridoConMismasCiudadesYFecha(updateRecorridoDto.CiudadOrigenId, updateRecorridoDto.CiudadDestinoId, updateRecorridoDto.FechaViaje, id))
        {
            return ResponseDto.Error("Ya existe un recorrido con las mismas ciudades y fecha.");
        }

        recorrido.CiudadOrigenId = updateRecorridoDto.CiudadOrigenId;
        recorrido.CiudadDestinoId = updateRecorridoDto.CiudadDestinoId;
        recorrido.FechaViaje = updateRecorridoDto.FechaViaje;

        recorrido.Codigo = await GenerateCodigo(updateRecorridoDto.CiudadOrigenId, updateRecorridoDto.CiudadDestinoId, updateRecorridoDto.FechaViaje);
        recorrido.Distancia = await CalculateDistance(updateRecorridoDto.CiudadOrigenId, updateRecorridoDto.CiudadDestinoId);
        recorrido.Precio = CalculatePrice(recorrido.Distancia, updateRecorridoDto.FechaViaje);

        recorrido.ClienteRecorridos.Clear();
        if (updateRecorridoDto.Pasajeros != null && updateRecorridoDto.Pasajeros.Any())
        {
            foreach (var clienteId in updateRecorridoDto.Pasajeros)
            {
                var clienteDto = await _clienteRepository.GetClienteByIdAsync(clienteId);
                if (clienteDto == null)
                {
                    return ResponseDto.Error($"Cliente con ID {clienteId} no encontrado.");
                }

                var cliente = _mapper.Map<Cliente>(clienteDto);

                var existingCliente = await _context.Clientes.FindAsync(cliente.Id);
                if (existingCliente != null)
                {
                    _context.Entry(existingCliente).State = EntityState.Detached;
                }

                _context.Attach(cliente);

                recorrido.ClienteRecorridos.Add(new ClienteRecorrido { ClienteId = clienteId, Cliente = cliente });
            }
        }

        await _recorridoRepository.UpdateAsync(recorrido);
        var recorridoDto = _mapper.Map<RecorridoDto>(recorrido);
        return ResponseDto.Ok(recorridoDto);
    }
    public async Task<ResponseDto> UpdateEstadoAsync(int id, EstadoRecorrido nuevoEstado)
    {
        var recorrido = await _recorridoRepository.GetByIdAsync(id);
        if (recorrido == null)
        {
            return ResponseDto.Error("Recorrido no encontrado.");
        }

        if (recorrido.Eliminado)
        {
            return ResponseDto.Error("No se puede actualizar el estado de un recorrido eliminado.");
        }

        if (!Enum.IsDefined(typeof(EstadoRecorrido), nuevoEstado))
        {
            return ResponseDto.Error("Estado no válido.");
        }

        if (recorrido.Estado == EstadoRecorrido.Pendiente && nuevoEstado != EstadoRecorrido.EnProgreso)
        {
            return ResponseDto.Error("El estado sólo puede ser actualizado a 'En progreso' desde 'Pendiente'.");
        }

        if (recorrido.Estado == EstadoRecorrido.EnProgreso && nuevoEstado != EstadoRecorrido.Finalizado)
        {
            return ResponseDto.Error("El estado sólo puede ser actualizado a 'Finalizado' desde 'En progreso'.");
        }

        if (recorrido.Estado != EstadoRecorrido.Pendiente && recorrido.Estado != EstadoRecorrido.EnProgreso)
        {
            return ResponseDto.Error("Sólo se pueden actualizar recorridos en estado 'Pendiente' o 'En progreso'.");
        }

        recorrido.Estado = nuevoEstado;
        await _recorridoRepository.UpdateAsync(recorrido);

        return ResponseDto.Ok(null, "Estado actualizado con éxito");
    }

    public async Task<ResponseDto> DeleteAsync(int id)
    {
        var recorrido = await _recorridoRepository.GetByIdAsync(id);
        if (recorrido == null)
        {
            return ResponseDto.Error("Recorrido no encontrado.");
        }

        if (recorrido.Estado != EstadoRecorrido.Pendiente)
        {
            return ResponseDto.Error("Sólo se pueden eliminar recorridos en estado 'Pendiente'.");
        }

        await _recorridoRepository.DeleteAsync(id);
        return ResponseDto.Ok(null,"Se eliminó con éxito");
    }

    public async Task<string> GenerateCodigo(int ciudadOrigenId, int ciudadDestinoId, DateTime fechaViaje)
    {
        var ciudadOrigen = await _ciudadRepository.GetCiudadByIdAsync(ciudadOrigenId);
        if (ciudadOrigen == null)
        {
            throw new ArgumentException($"Ciudad origen con ID {ciudadOrigenId} no encontrada.");
        }

        var ciudadDestino = await _ciudadRepository.GetCiudadByIdAsync(ciudadDestinoId);
        if (ciudadDestino == null)
        {
            throw new ArgumentException($"Ciudad destino con ID {ciudadDestinoId} no encontrada.");
        }

        string codigoOrigen = ciudadOrigen.Descripcion.Length >= 3 ? ciudadOrigen.Descripcion.Substring(0, 3).ToUpper() : ciudadOrigen.Descripcion.ToUpper();
        string codigoDestino = ciudadDestino.Descripcion.Length >= 3 ? ciudadDestino.Descripcion.Substring(0, 3).ToUpper() : ciudadDestino.Descripcion.ToUpper();
        string fechaCodigo = fechaViaje.ToString("ddMMyyyy");

        return $"{codigoOrigen}{codigoDestino}{fechaCodigo}";
    }

    public async Task<double> CalculateDistance(int ciudadOrigenId, int ciudadDestinoId)
    {
        var ciudadOrigen = await _ciudadRepository.GetCiudadByIdAsync(ciudadOrigenId);
        var ciudadDestino = await _ciudadRepository.GetCiudadByIdAsync(ciudadDestinoId);

        if (ciudadOrigen == null || ciudadDestino == null)
        {
            throw new ArgumentException("Ciudad de origen o destino no encontrada.");
        }

        var R = 6371;
        var latOrigenRad = ciudadOrigen.Latitud * (Math.PI / 180);
        var latDestinoRad = ciudadDestino.Latitud * (Math.PI / 180);
        var deltaLat = (ciudadDestino.Latitud - ciudadOrigen.Latitud) * (Math.PI / 180);
        var deltaLon = (ciudadDestino.Longitud - ciudadOrigen.Longitud) * (Math.PI / 180);

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(latOrigenRad) * Math.Cos(latDestinoRad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distancia = R * c;

        return Math.Round(distancia, 1);
    }

    public double CalculatePrice(double distancia, DateTime fechaViaje)
    {
        double factor = 1.8;
        if (fechaViaje.DayOfWeek == DayOfWeek.Sunday || EsFeriado(fechaViaje))
        {
            factor = 2.6;
        }
        else if (fechaViaje.DayOfWeek == DayOfWeek.Tuesday || fechaViaje.DayOfWeek == DayOfWeek.Thursday)
        {
            factor = 1.3;
        }
        double precio = distancia * factor;
        return Math.Round(precio, 1);
    }

    private bool EsFeriado(DateTime fecha)
    {
        //Feriados en Perú
        var feriados = new List<DateTime>

            {
                new DateTime(fecha.Year, 1, 1),    // 1 de enero
                 new DateTime(fecha.Year, 3, 28),   // 28 de marzo
                new DateTime(fecha.Year, 3, 29),   // 29 de marzo
                 new DateTime(fecha.Year, 5, 1),    // 1 de mayo
                 new DateTime(fecha.Year, 6, 7),    // 7 de junio
                 new DateTime(fecha.Year, 6, 29),   // 29 de junio
                 new DateTime(fecha.Year, 7, 23),   // 23 de julio
                 new DateTime(fecha.Year, 7, 28),   // 28 de julio
                 new DateTime(fecha.Year, 7, 29),   // 29 de julio
                 new DateTime(fecha.Year, 8, 6),    // 6 de agosto
                 new DateTime(fecha.Year, 8, 30),   // 30 de agosto
                 new DateTime(fecha.Year, 10, 8),   // 8 de octubre
                 new DateTime(fecha.Year, 11, 1),   // 1 de noviembre
                 new DateTime(fecha.Year, 12, 8),   // 8 de diciembre
                 new DateTime(fecha.Year, 12, 9),   // 9 de diciembre
                new DateTime(fecha.Year, 12, 25)   // 25 de diciembre
            };

        return feriados.Contains(fecha.Date);
    }

    public async Task<bool> ExisteRecorridoConMismasCiudadesYFecha(int ciudadOrigenId, int ciudadDestinoId, DateTime fechaViaje, int? excludeId = null)
    {
        var recorridos = await _context.Recorridos
       .Where(r => r.CiudadOrigenId == ciudadOrigenId
                && r.CiudadDestinoId == ciudadDestinoId
                && r.FechaViaje.Date == fechaViaje.Date
                && (!excludeId.HasValue || r.Id != excludeId.Value)
                && !r.Eliminado)
       .ToListAsync();

        return recorridos.Any();
    }
    public async Task<IEnumerable<RecorridoDto>> GetRecorridosByEstadoAsync(EstadoRecorrido? estado)
    {
        var query = _context.Recorridos
                        .Include(r => r.CiudadOrigen)
                        .Include(r => r.CiudadDestino)
                        .Include(r => r.ClienteRecorridos)
                            .ThenInclude(cr => cr.Cliente)
                        .Where(r => !r.Eliminado)
                        .AsQueryable();

        if (estado.HasValue)
        {
            query = query.Where(r => r.Estado == estado.Value);
        }

        var recorridos = await query.ToListAsync();
        return _mapper.Map<IEnumerable<RecorridoDto>>(recorridos);
    }

    public async Task<IEnumerable<RecorridoDto>> GetRecorridosByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        var query = _context.Recorridos
                        .Include(r => r.CiudadOrigen)
                        .Include(r => r.CiudadDestino)
                        .Include(r => r.ClienteRecorridos)
                            .ThenInclude(cr => cr.Cliente)
                        .Where(r => r.FechaViaje >= fechaInicio && r.FechaViaje <= fechaFin && !r.Eliminado)
                        .AsQueryable();

        var recorridos = await query.ToListAsync();

        return _mapper.Map<IEnumerable<RecorridoDto>>(recorridos);

    }
    public async Task<RecorridoDto> GetRecorridoByCodigoAsync(string codigo)
    {
        var recorrido = await _context.Recorridos
                                 .Include(r => r.CiudadOrigen)
                                 .Include(r => r.CiudadDestino)
                                 .Include(r => r.ClienteRecorridos)
                                     .ThenInclude(cr => cr.Cliente)
                                 .FirstOrDefaultAsync(r => r.Codigo == codigo && !r.Eliminado);
        if (recorrido == null)
        {
            return null;
        }
        return _mapper.Map<RecorridoDto>(recorrido);
    }

}