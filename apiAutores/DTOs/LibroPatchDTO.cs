using apiAutores.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [Required]
        [StringLength(120)]
        [FirstCharUpper]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
