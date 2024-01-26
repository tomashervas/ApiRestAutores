using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper )
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroGetDTO>> GetLibro(int id)
        {
            if (id == 0) 
            {
                return BadRequest("El id no puede ser 0");
            }

            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroGetDTO>(libro);

            return libroDTO;
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<LibroGetDTO>> GetLibroByTitle(string title)
        {
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Titulo.Contains(title));
            if (libro == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroGetDTO>(libro);

            return libroDTO;
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroGetDTO>>> GetLibros()
        {
            var libros = await context.Libros.ToListAsync();

            var librosDTO = mapper.Map<List<LibroGetDTO>>(libros);
            return librosDTO;

        }

        [HttpPost]
        public async Task<ActionResult> SaveLibro(LibroDTO libroDTO)
        {
            //var existsautor = await context.autores.anyasync(x => x.id == libro.autorid);
            //if (!existsautor)
            //{
            //    return badrequest($"no existe ningun autor con id: {libro.autorid}");
            //}

            var libro = mapper.Map<Libro>(libroDTO);

            context.Add(libro);
            await context.SaveChangesAsync();

            return Ok(libro);
        }


    }
}
