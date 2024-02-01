using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "getLibrobyIdv1")]
        public async Task<ActionResult<LibroGetDTO>> GetLibro(int id)
        {
            if (id == 0)
            {
                return BadRequest("El id no puede ser 0");
            }

            var libro = await context.Libros.Include(x => x.AutoresLibros).ThenInclude(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

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
            if (libroDTO.AutoresIds == null || libroDTO.AutoresIds.Count == 0) { return BadRequest("No se puede crear un libro sin autores"); }

            var autoresIds = await context.Autores.Where(x => libroDTO.AutoresIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            if (libroDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroDTO);

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            // var libroDTORespuesta = mapper.Map<LibroGetDTO>(libro);
            //TODO: retornar libroDTORespuesta
            return CreatedAtRoute("getLibrobyIdv1", new { id = libro.Id }, libro);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroDTO libroDTO)
        {
            var libroDB = await context.Libros.Include(x => x.AutoresLibros).FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroDTO, libroDB);

            AsignarOrdenAutores(libroDB);

            context.Update(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }
            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var isValid = TryValidateModel(libroDTO);
            if (!isValid)
            {
                return BadRequest(ModelState);
            };
            mapper.Map(libroDTO, libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Libros.AnyAsync(autor => autor.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }
    }
}
