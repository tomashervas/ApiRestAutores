using apiAutores.DTOs;
using apiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros/{libroId:int}/comentario")]
    public class ComentarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentarioController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro) { return NotFound(); }

            var comentariosDTO = await context.Commentarios.Where(x => x.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentariosDTO);

        }

        [HttpGet("{id:int}", Name = "getComentarioByIdv1")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int id)
        {
            var comentario = await context.Libros.AnyAsync(x => x.Id == id);
            if (!comentario) { return NotFound(); }


            return mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var email = emailClaim!.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario!.Id;

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro) { return NotFound(); }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);

            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;

            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTOresponse = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("getComentarioByIdv1", new { id = comentario.Id, libroId }, comentarioDTOresponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Commentarios.AnyAsync(x => x.Id == id);
            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.Id = id;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
