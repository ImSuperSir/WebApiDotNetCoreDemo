namespace _002_WebApiAutores.DTOs
{
    public class LibroDTO
    {

        public int Id { get; set; }

        public string Titulo     { get; set; }

        public DateTime FechaPublicacion { get; set; }


        //public List<ComentarioDTO> Comentarios { get; set; }, se puede establecer, decir que se prefiere que sea lazyload :-)


    }
}
