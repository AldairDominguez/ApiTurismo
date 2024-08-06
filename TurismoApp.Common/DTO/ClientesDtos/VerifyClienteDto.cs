namespace TurismoApp.Common.DTO.ClientesDtos;

public class VerifyClienteDto
{
    public bool Verificado { get; set; }
    public required string VerificacionToken { get; set; }
    public DateTime? FechaVerificacion { get; set; }
}