using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.DTOs
{
    public class AutorCreacionDTO
    {

        [PrimeraLetraMayuscula]
        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(maximumLength: 60, ErrorMessage = "the {0} must be a string with a maximum length of {1}")]
        public string Name { get; set; }

    }
}
