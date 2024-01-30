using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class LibroGetDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<ComentarioDTO> Comentarios { get; set; }
        public List<AutorGetDTO> Autores { get; set; }
    }
}
