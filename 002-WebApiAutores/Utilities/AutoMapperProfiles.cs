using _002_WebApiAutores.DTOs;
using _002_WebApiAutores.Entidades;
using AutoMapper;

namespace _002_WebApiAutores.Utilities
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.AutoresDeLibro, y => y.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();     //reverse map is to make the inverse mapping

            CreateMap<Libro, LibroDTO>();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();


        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro.AutoresDeLibro  == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresDeLibro)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorlibro.AutorId,
                    Name = autorlibro.Autor.Name
                });
            }

            return resultado;
        }

        public List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTo, Libro libro)
        {
            var lResultado = new List<AutorLibro>();

            if (libroCreacionDTo.AutoresIds == null )
            {
                return lResultado;
            }

            foreach (var item in libroCreacionDTo.AutoresIds)
            {
                lResultado.Add(new AutorLibro() { AutorId = item });
            }

            return lResultado;
        
        }
    }
}
