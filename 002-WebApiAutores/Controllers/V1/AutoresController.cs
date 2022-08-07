using _002_WebApiAutores.DTOs;
using _002_WebApiAutores.Entidades;
using _002_WebApiAutores.Filters;
using _002_WebApiAutores.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores.Controllers.V1
{

    [ApiController]   //Me permite realizar validaciones automaticas, respecto a la data recibira en el controlador
    //[Route("api/autores")]          //api/autores
    [Route("api/v{version:apiversion}/autores")]
    [ApiVersion("1.0")]
    //[Authorize]  // trabada con services.AddAuthentication(JwtBearerDefaults.AuthenticationSchema).AddJwtBearers....
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly IMapper mapper;

        public ApplicationDbContext Context { get; }

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            this.mapper = mapper;
        }

        #region OldComments
        /*
    Whe we use ActionResult as a response, we ensure
    we can response any class derived from ActionResult or a particular type,
    In this case, the particular type es "Autor" class

    With IActionResult, the big difference is that in this case
    we can return any type, not just "Autor" Class

    ActionResult, is more restrictive and maybe better from my point of view.
 */
        #endregion


        [HttpGet]
        //[HttpGet("listado")]   //api/autores/listado
        //[HttpGet("/listado")]  //listado
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        //[ResponseCache(Duration = 10)]
        [AllowAnonymous]  //this es the exception for the authentication
        //[MapToApiVersion("2.0")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            ////Logger.LogInformation("Informationlevel: this is just a test for logging");
            ////Logger.LogWarning("Warning level: Este es un mensaje de warning");
            //var lAutores = await Context.Autores.ToListAsync(); //.Include(x => x.Libros).ToListAsync();
            //return mapper.Map<List<AutorDTO>>(lAutores);

            var queryable = Context.Autores.AsQueryable();
            await HttpContext.InsertaParametrosPaginacionEnCabecera(queryable);

            var autores = await queryable.OrderBy(autor => autor.Name).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);


        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]       //when you put  ":int", this a restriction, for get just integers to to this endpoint
        //[HttpGet("{id:int}/{nombre}")]       //It can be added any parameters, there IS NO restructions for strings
        //[HttpGet("{id:int}/{nombre?}")]       //With the question mark im setting that the "nombre" param is optional
        //[HttpGet("{id:int}/{nombre=unvalorx}")]       //In this way, im putting a default value for thar route variable
        public async Task<ActionResult<AutorDTO>> Get(int id) //, string nombre)
        {


            //throw new NotImplementedException("Esta es una excepcion de prueba para probar el filtro global de excepciones");
            var lautor = await Context.Autores.FirstOrDefaultAsync(x => x.Id == id);


            //if we do not put this validation, the reques is not going to show the error, just a blank page, 
            //so we need to specify it it was not foud, must show the corrrect response code
            if (null == lautor)
                return NotFound();

            return mapper.Map<AutorDTO>(lautor);

            //return Ok(lautor);
        }


        [HttpGet("{nombre}")]       //api/autores/xxxx  when you put it into "{}", the is named route variable
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var lautores = await Context.Autores.Where(x => x.Name.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorDTO>>(lautores);

            //if (null == lautor)
            //    return NotFound();

            //return Ok(lautor);
        }

        #region OldCodePrimerAutor
        //[HttpGet("primerautor")]        //api/autores/primerautor
        //public async Task<ActionResult<Autor>> PrimerAutor()
        //{
        //    return await Context.Autores.FirstOrDefaultAsync();
        //} 
        #endregion

        [HttpPost]
        public async Task<ActionResult<List<AutorDTO>>> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {

            var lAutorExiste = await Context.Autores.AnyAsync(x => x.Name == autorCreacionDTO.Name);

            if (lAutorExiste)
            {
                return BadRequest($"El autor con el nombre {autorCreacionDTO.Name}, ya existe en la base de datos.");
            }

            //var lAutorToCreate = new Autor { Name = autorCreacionDTO.Nombre };

            var lAutorToCreate = mapper.Map<Autor>(autorCreacionDTO);

            Context.Add(lAutorToCreate);
            await Context.SaveChangesAsync();

            var lAutorDTO = mapper.Map<AutorDTO>(lAutorToCreate);

            return CreatedAtRoute("obtenerAutor", new { id = lAutorToCreate.Id }, lAutorDTO); //Ok();
        }

        [HttpPut("{id:int}")]       //api/autores/1
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var lExiste = await Context.Autores.AnyAsync(x => x.Id == id);

            if (!lExiste)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            var lAutorToUpdate = mapper.Map<Autor>(autorCreacionDTO);
            lAutorToUpdate.Id = id;

            Context.Update(lAutorToUpdate);
            await Context.SaveChangesAsync();

            return NoContent(); // este es un 204, not content  //Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await Context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            Context.Remove(new Autor() { Id = id });
            await Context.SaveChangesAsync();
            return Ok();

        }

    }
}
