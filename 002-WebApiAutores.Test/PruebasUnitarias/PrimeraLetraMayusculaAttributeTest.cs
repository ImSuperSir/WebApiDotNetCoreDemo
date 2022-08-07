using _002_WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.Test.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTest
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveUnError()
        {
            //preparation
            var lPrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var lValor = "lauro";
            var lValidationContex = new ValidationContext(new { Nombre = lValor });
            //execution
            var lResultado = lPrimeraLetraMayuscula.GetValidationResult(lValor, lValidationContex);
            //validation
            Assert.AreEqual("The first letter must be a capital letter.", lResultado.ErrorMessage);
        }


        [TestMethod]
        public void ValorNulo_NoDevuelveUnError()
        {
            //preparation
            var lPrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string lValor = null;
            var lValidationContex = new ValidationContext(new { Nombre = lValor });
            //execution
            var lResultado = lPrimeraLetraMayuscula.GetValidationResult(lValor, lValidationContex);
            //validation

            Assert.IsNull(lResultado);   //ValidationResult es nulo cuando es exitosa
        }


        [TestMethod]
        public void ValorConMayuscula_NoDevuelveUnError()
        {
            //preparation
            var lPrimeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string lValor = "Lauro";
            var lValidationContex = new ValidationContext(new { Nombre = lValor });
            //execution
            var lResultado = lPrimeraLetraMayuscula.GetValidationResult(lValor, lValidationContex);
            //validation

            Assert.IsNull(lResultado);   //ValidationResult es nulo cuando es exitosa
        }
    }
}