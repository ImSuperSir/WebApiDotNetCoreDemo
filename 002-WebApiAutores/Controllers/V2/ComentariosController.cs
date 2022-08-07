using _002_WebApiAutores.DTOs;
using _002_WebApiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores.Controllers.V2
{


    /*
     * Este es un controlador de un recurso dependiente
     * Los comentarios son totalmente dependientes del libro
     * Así que para definirlo usamos este tipo de ruta
     */
    [ApiController]
    //[Route("api/libros/{libroId:int}/comentarios")]
    [Route("api/v{version:apiversion}/libros/{libroId:int}/comentarios")]
    [ApiVersion("2.0")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper
            , UserManager<IdentityUser> userManager)

        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;  //para poder consultar Identity
        }


        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)  /*Es imprtante que el nombre del parametro, 
                                                                                * sea igual que el nombre del parametro de 
                                                                                * la url de controlador, en este caso particula de la dependencia de recursos*/
        {

            var lExisteLibro = await context.Libros.FirstOrDefaultAsync(x => x.Id == libroId);
            if (null == lExisteLibro)
            {
                return NotFound($"No exite el id del libro: {libroId}.");
            }

            var lComentarios = await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(lComentarios);

        }

        [HttpGet("id:int", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int id)
        {
            //var lExisteLibro = await context.Comentarios.AnyAsync(x => x.Id == id);

            //if (!lExisteLibro)
            //{
            //    return NotFound("The book's commentary you are looking for does not exist.");
            //}

            var lComentario = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (null == lComentario)
            {
                return NotFound("The book's commentary you are looking for does not exist.");
            }


            return mapper.Map<ComentarioDTO>(lComentario);

        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {

            var lEmailClain = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var lEmail = lEmailClain.Value;

            var lUsuario = await userManager.FindByEmailAsync(lEmail);
            var lUsuarioId = lUsuario.Id;

            var lExisteLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!lExisteLibro)
            {
                return NotFound("No existe el libro.");
            }

            var lComentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            lComentario.LibroId = libroId;
            lComentario.UsuarioId = lUsuarioId;

            context.Comentarios.Add(lComentario);


            await context.SaveChangesAsync();

            var lComentarioDTO = mapper.Map<ComentarioDTO>(lComentario);


            return CreatedAtRoute("obtenerComentario", new { id = lComentario.Id, libroId }, lComentarioDTO); //Ok();

        }

        [HttpPut]
        public async Task<ActionResult> Put(ComentarioCreacionDTO comentarioCreacionDTO, int id, int libroId)
        {
            var lExisteLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!lExisteLibro)
            {
                return NotFound("El libro al cual se le require actualizar el comentario no existe.");

            }

            var lExisteComentario = await context.Comentarios.AnyAsync(x => x.Id == id);

            if (!lExisteComentario)
            {
                return NotFound("The comment you want to update, does not exist.");
            }


            var lCommentToUpdate = mapper.Map<Comentario>(comentarioCreacionDTO);
            lCommentToUpdate.LibroId = libroId;
            lCommentToUpdate.Id = id;


            context.Update(lCommentToUpdate);
            await context.SaveChangesAsync();

            return NoContent();

        }

    }
}
