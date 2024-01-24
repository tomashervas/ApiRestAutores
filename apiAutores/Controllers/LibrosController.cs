using apiAutores.Entidades;
using apiAutores.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            if (id == 0) 
            {
                return BadRequest("El id no puede ser 0");
            }

            var libro = await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<Libro>> GetLibroByTitle(string title)
        {
            var libro = await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Titulo.Contains(title));
            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetLibros()
        {
            return await context.Libros.Include(x=>x.Autor).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> SaveLibro(Libro libro)
        {
            var existsAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existsAutor)
            {
                return BadRequest($"No existe ningun autor con id: {libro.AutorId}");
            }

            context.Add(libro);
            await context.SaveChangesAsync();

            return Ok(libro);
        }


    }
}
