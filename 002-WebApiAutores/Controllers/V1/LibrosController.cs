using _002_WebApiAutores.DTOs;
using _002_WebApiAutores.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores.Controllers.V1
{

    [ApiController]
    //[Route("api/libros")]
    [Route("api/v{version:apiversion}/libros")]
    [ApiVersion("1.0")]
    public class LibrosController : Controller
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public ApplicationDbContext Context { get; }
        public LibrosController(ApplicationDbContext Context, IMapper mapper, IConfiguration configuration)
        {
            this.Context = Context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        [HttpGet("configuraciones")]  //api/libros/configuraciones
        public async Task<ActionResult> ObtenerConfiguracion()
        {

            return Ok(configuration["MisPruebasConfiguracionDos"].ToString());
        }

        [HttpGet("{id:int}", Name = "obenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var lLibro = await Context.Libros
                .Include(x => x.AutoresDeLibro)
                .ThenInclude(x => x.Autor)
                //.Include( z => z.Comentarios)   /*Se puede omitir si es que se prefiere que la carga de los comentarios sea por lazyload */
                .FirstOrDefaultAsync(x => x.Id == id);//. .ToListAsync<Autor>(); // .Include( x => x.Autor) .FirstOrDefaultAsync(x => x.Id == id);


            if (null == lLibro)
                return NotFound();


            lLibro.AutoresDeLibro = lLibro.AutoresDeLibro.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOConAutores>(lLibro);

        }


        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDTO)
        {
            //var existeAutor = await Context.Autores.AnyAsync( x => x.Id == libro.AutorId);

            //if (!existeAutor)
            //{
            //    return BadRequest($"No existe el autor de id: {libro.AutorId}");
            //}

            if (libroDTO.AutoresIds == null || libroDTO.AutoresIds.Count == 0)  // el count es necesario, por si se inicializa el list en el DTO
            {
                return BadRequest($"No se puede crear un libro sin autores.");
            }

            var lAutoresIds = await Context.Autores
                .Where(x => libroDTO.AutoresIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            if (libroDTO.AutoresIds.Count != lAutoresIds.Count)
            {
                return BadRequest($"No existe al menos uno de los autores enviados.");
            }


            var lLibro = mapper.Map<Libro>(libroDTO);

            OrdernarLibros(lLibro);

            Context.Libros.Add(lLibro);
            await Context.SaveChangesAsync();


            var lLibroDTO = mapper.Map<LibroDTO>(lLibro);
            return CreatedAtRoute("obenerLibro", new { id = lLibro.Id }, lLibroDTO); //Ok();
        }

        private static void OrdernarLibros(Libro lLibro)
        {
            if (lLibro.AutoresDeLibro != null & lLibro.AutoresDeLibro.Count > 0)
            {
                for (int i = 0; i < lLibro.AutoresDeLibro.Count; i++)
                {
                    lLibro.AutoresDeLibro[i].Orden = i;
                }
            }
        }



        /// <summary>
        /// In this method the principal idea, it is tha we are goin to update 
        /// the bood and the authos, so, we must take care abour the DTO
        /// id you do not have the appropiate DTO, do not hesitate in create a new one
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libroCreacionDTo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTo)
        {
            //do the first step it is to get from the database all the informacion
            // that we can update, but be careful, for this case, the information 
            // it not heavy, so, if that was the case, maybe it can be a good idea
            // to do the change in separate methods

            var lLibroFromDatabase = await Context.Libros.Include(x => x.AutoresDeLibro).FirstOrDefaultAsync(x => x.Id == id);

            if (lLibroFromDatabase == null)
            {
                return NotFound();
            }

            lLibroFromDatabase = mapper.Map(libroCreacionDTo, lLibroFromDatabase);

            OrdernarLibros(lLibroFromDatabase);

            await Context.SaveChangesAsync();

            return NoContent();


        }


        [HttpPatch]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await Context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (null == libroDB) return NotFound();


            var libroPatchDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroPatchDTO, ModelState);

            var lEsvalido = TryValidateModel(libroPatchDTO);

            if (!lEsvalido) return BadRequest(ModelState);

            mapper.Map(libroPatchDTO, libroDB);

            await Context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("id:int")] //api/libros/2  //the difference betwen the get and the delete is the http verb, it makes the difference on the action to be taken
        public async Task<ActionResult> DeleteBook(int id)
        {
            //first of all

            var lLibroExiste = await Context.Libros.AnyAsync(x => x.Id == id);

            if (!lLibroExiste) return NotFound();


            Context.Libros.Remove(new Libro() { Id = id });

            await Context.SaveChangesAsync();

            return NoContent();// Ok();


        }
    }
}
