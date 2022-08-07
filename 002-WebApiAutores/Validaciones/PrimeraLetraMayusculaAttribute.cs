using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // i jsut wanna validate, that in case the l
            //string is valid, then checkit the rule.
            //Maybe, there is another attribute to ckeck that is required
            // i want avoid a doble check
            if (null == value || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var lPrimeraLetra = value.ToString()[0].ToString();

            if (lPrimeraLetra != lPrimeraLetra.ToUpper())
            {
                return new ValidationResult("The first letter must be a capital letter.");
            }

            return ValidationResult.Success;
        }

    }
}
