using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class ComentarioCreacionDTO
    {
        [Required]
        public string Contenido { get; set; }
    }
}