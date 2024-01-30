using apiAutores.Entidades;
using Microsoft.EntityFrameworkCore;

namespace apiAutores
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AutorLibro>().HasKey(x => new { x.AutorId, x.LibroId });
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Commentarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}
