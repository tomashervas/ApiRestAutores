using apiAutores.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class AutorDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        [FirstCharUpper]
        public string Name { get; set; }
    }
}
