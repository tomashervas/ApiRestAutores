using apiAutores.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [FirstCharUpper]
        public  string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
