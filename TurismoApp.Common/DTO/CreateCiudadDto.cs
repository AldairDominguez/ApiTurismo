namespace TurismoApp.Common.DTO;

public class CreateCiudadDto
{
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int DepartamentoId { get; set; }
}