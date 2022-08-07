using Microsoft.AspNetCore.Identity;

namespace _002_WebApiAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }

        public int LibroId { get; set; }

        /*Propiedad de navegacion
         Nos permite realizar joins facilmente
         */
        public Libro Libro { get; set; }


        public string UsuarioId { get; set; }

        //public IdentityUser Usuario { get; set; } //as this field is not in the same database, 



    }
}
