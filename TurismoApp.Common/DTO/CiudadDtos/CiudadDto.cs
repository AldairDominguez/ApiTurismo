namespace TurismoApp.Common.DTO.CiudadDtos;

public class CiudadDto
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int DepartamentoId { get; set; }
    public string DepartamentoDescripcion { get; set; }
    public string Descripcion { get; set; }

}