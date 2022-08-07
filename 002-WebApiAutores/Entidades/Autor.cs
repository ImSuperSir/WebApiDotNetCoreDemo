using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _002_WebApiAutores.Entidades
{
    public class Autor  //: IValidatableObject
    {

        #region OldCode
        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Name))
        //    {
        //        var lPrimeraLetra = Name[0].ToString();
        //        if (lPrimeraLetra != lPrimeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser maypuscula",
        //                  new string[] { nameof(Name) }
        //                    );
        //        }
        //    }
        //    /* here it can continue the next validations of the 
        //     model */
        //    //if (Menor > Mayor)
        //    //{
        //    //    yield return new ValidationResult("El menor no puede ser mayor que el mayor",
        //    //       new string[] { nameof(Menor) }
        //    //        );
        //    //}
        //}

        //[NotMapped]   //System.ComponentModel.DataAnnotations.Schema;
        //public int Mayor { get; set; }
        //public int Menor { get; set; }
        /*this is done by the model binding, and allow us to do validations by default
             usign "{0}", sets the name of the property automatically

                using "{1}", in this case is the value of the restriction of length, and its taken automatically
             */ 
        #endregion


        [PrimeraLetraMayuscula]
        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(maximumLength: 60, ErrorMessage = "the {0} must be a string with a maximum length of {1}")]
        public string Name { get; set; }


        #region OldCodeDos
        //[Range(18, 20)]
        //[NotMapped]
        //public int Edad { get; set; } 
        #endregion


        public int Id { get; set; }

        public List<AutorLibro> AutorLibro { get; set; }





        //public List<Libro> Libros { get; set; }


    }
}
