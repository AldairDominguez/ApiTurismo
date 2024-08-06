using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Common.DTO;

public class CreateClienteDto
{
    [Required]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "El DNI debe contener solo números.")]
    public string Dni { get; set; }
    [Required]
    public string Nombres { get; set; }
    [Required]
    public string Apellidos { get; set; }
    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|org|net|edu|pe)$", ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Correo { get; set; }
}