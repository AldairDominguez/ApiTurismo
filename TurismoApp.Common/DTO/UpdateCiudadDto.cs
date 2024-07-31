namespace TurismoApp.Common.DTO;

public class UpdateCiudadDto
{
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int DepartamentoId { get; set; }
    public string Descripcion { get; set; }
}