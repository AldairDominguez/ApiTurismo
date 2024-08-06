namespace TurismoApp.Common.DTO.ClientesDtos;

public class ClienteDto
{
    public int Id { get; set; }
    public string Dni { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }

    public bool Verificado { get; set; }
    public string VerificacionToken { get; set; }
    public DateTime? FechaVerificacion { get; set; }
    public bool Eliminado { get; set; }
}

