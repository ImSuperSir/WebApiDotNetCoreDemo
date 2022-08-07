using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.Entidades
{
    public class Libro
    {

        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250 ,ErrorMessage = "{El campo {0}, tiene una longitud maxima de {1}}")]
        public string Titulo { get; set; }

        //public int AutorId { get; set; }

        //public Autor Autor { get; set; }

        public DateTime? FechaPublicacion { get; set; }  //nullable, ya que se realizó cuando ya existian registros en la base de datos


        public List<Comentario> Comentarios { get; set; }  /*esta es una propiedad de navegacion, me permite hacer joins en linq facilmente, 
                                                            * Comentario, es una tabla de la base de datos*/
        public List<AutorLibro> AutoresDeLibro { get; set; }

    }
}
