using System.ComponentModel.DataAnnotations;

namespace apiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public  string Titulo { get; set; }
    }
}
