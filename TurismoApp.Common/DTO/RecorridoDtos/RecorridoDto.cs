using TurismoApp.Common.Enums;

namespace TurismoApp.Common.DTO.RecorridoDtos;

public class RecorridoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaViaje { get; set; }
    public EstadoRecorrido Estado { get; set; }
    public int IdCiudadOrigen { get; set; }
    public string CiudadOrigen { get; set; }
    public int IdCiudadDestino { get; set; }
    public string CiudadDestino { get; set; }
    public double Distancia { get; set; }
    public double Precio { get; set; }
    public List<PasajeroResponseDto> Pasajeros { get; set; }
}