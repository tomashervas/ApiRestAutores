using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorGetDTO>>> GetAutores()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<List<AutorGetDTO>>(autores);
            return autoresDTO;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorGetDTO>> GetAutor(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null) { return NotFound(); }
            var autorDTO = mapper.Map<AutorGetDTO>(autor);
            return autorDTO;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AutorGetDTO>>> GetAutorByName([FromQuery] string name, [FromHeader] int valor)
        {
            var autores =  await context.Autores.Where(x => x.Name.Contains(name)).ToListAsync();
            var autoresDTO = mapper.Map<List<AutorGetDTO>>(autores);
            if (autoresDTO.Count == 0)
            {
                return NotFound();
            }

            return autoresDTO;
        }

        [HttpPost]
        public async Task<ActionResult> SaveAutor(AutorDTO autorDTO)
        {
            //validación en controlador
            var exists = await context.Autores.AnyAsync(x => x.Name == autorDTO.Name);
            if (exists)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorDTO.Name}");
            }

            var autor = mapper.Map<Autor>(autorDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ModifyAutor(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var exists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAutor(int id)
        {
            var exists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id, Name = "Doe"});
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
