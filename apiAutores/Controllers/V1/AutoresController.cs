﻿using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AutorGetDTO>>> GetAutores()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<List<AutorGetDTO>>(autores);
            return autoresDTO;
        }

        [HttpGet("{id:int}", Name = "getAutorByIdv1")]
        public async Task<ActionResult<AutorGetDTO>> GetAutor(int id)
        {
            var autor = await context.Autores.Include(x => x.AutoresLibros).ThenInclude(x => x.Libro).FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null) { return NotFound(); }
            var autorDTO = mapper.Map<AutorGetDTO>(autor);
            return autorDTO;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AutorGetDTO>>> GetAutorByName([FromQuery] string name, [FromHeader] int valor)
        {
            var autores = await context.Autores.Where(x => x.Name.Contains(name)).ToListAsync();
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

            var autorDTOReturn = mapper.Map<AutorGetDTO>(autor);

            return CreatedAtRoute("getAutorByIdv1", new { id = autor.Id }, autorDTOReturn);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ModifyAutor(AutorDTO autorDTO, int id)
        {
            var exists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> DeleteAutor(int id)
        {
            var exists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id, Name = "Doe" });
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/hola")]
        public ActionResult<string> Hola()
        {
            //var environment = Environment.GetEnvironmentVariable("HOLA");
            var environment = configuration["HOLAS"];
            return environment ?? "No hay esta variable de entorno";
        }

    }
}
