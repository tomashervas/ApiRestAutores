using apiAutores.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> GetAutores()
        {
            return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> SaveAutor(Autor autor)
        {
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
