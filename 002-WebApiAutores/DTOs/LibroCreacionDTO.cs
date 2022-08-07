using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {

        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250, ErrorMessage = "{El campo {0}, tiene una longitud maxima de {1}}")]
        [Required]
        public string Titulo { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public List<int> AutoresIds { get; set; }// = new List<int>();  /*Inicializar puede ocasionar un que las validaciones vs null fallen
                                                 // Se recomienda establecer como convencion que para los DTO no se inicializen
                                                 //     */


    }
}
