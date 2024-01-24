namespace apiAutores.Entidades
{
    public class Autor
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public List<Libro> Libros { get; set; }

    }
}
