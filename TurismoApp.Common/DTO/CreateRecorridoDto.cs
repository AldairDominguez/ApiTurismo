namespace TurismoApp.Common.DTO;

public class CreateRecorridoDto
{
    public DateTime FechaViaje { get; set; }
    public int CiudadOrigenId { get; set; }
    public int CiudadDestinoId { get; set; }
    public List<int> Pasajeros { get; set; }
}