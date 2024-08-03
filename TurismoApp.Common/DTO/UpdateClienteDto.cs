namespace TurismoApp.Common.DTO;

public class UpdateClienteDto
{
    public string Dni { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public bool Verificado { get; set; }
    public string VerificacionToken { get; set; }
}