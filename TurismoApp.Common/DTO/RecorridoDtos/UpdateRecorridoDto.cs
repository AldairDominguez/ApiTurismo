using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TurismoApp.Common.Enums;

namespace TurismoApp.Common.DTO;

public class UpdateRecorridoDto
{
    [Required]
    public DateTime FechaViaje { get; set; }

    [Required]
    public int CiudadOrigenId { get; set; }

    [Required]
    public int CiudadDestinoId { get; set; }

    public List<int> Pasajeros { get; set; }
}