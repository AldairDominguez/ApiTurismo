namespace TurismoApp.Infraestructure.Entities;

public class ClienteRecorrido
{
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public int RecorridoId { get; set; }
    public Recorrido Recorrido { get; set; }
}
