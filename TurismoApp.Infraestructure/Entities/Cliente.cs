namespace TurismoApp.Infraestructure.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string Dni { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public bool Verificado { get; set; }
    public string? VerificacionToken { get; set; }

    public List<ClienteRecorrido> ClienteRecorridos { get; set; }
}