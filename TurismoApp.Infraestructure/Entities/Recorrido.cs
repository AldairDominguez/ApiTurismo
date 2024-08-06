using TurismoApp.Common.Enums;

namespace TurismoApp.Infraestructure.Entities;

public class Recorrido
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaViaje { get; set; }
    public EstadoRecorrido Estado { get; set; }
    public int CiudadOrigenId { get; set; }
    public Ciudad CiudadOrigen { get; set; }
    public int CiudadDestinoId { get; set; }
    public Ciudad CiudadDestino { get; set; }
    public double Distancia { get; set; }
    public double Precio { get; set; }
    public bool Eliminado { get; set; }

    public List<ClienteRecorrido> ClienteRecorridos { get; set; } = new List<ClienteRecorrido>();
}