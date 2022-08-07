using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250, ErrorMessage = "{El campo {0}, tiene una longitud maxima de {1}}")]
        [Required]
        public string Titulo { get; set; }

        public DateTime FechaPublicacion { get; set; }

    }
}
