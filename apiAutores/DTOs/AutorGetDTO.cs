namespace apiAutores.DTOs
{
    public class AutorGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<LibroGetDTO> Libros { get; set; }
    }
}
