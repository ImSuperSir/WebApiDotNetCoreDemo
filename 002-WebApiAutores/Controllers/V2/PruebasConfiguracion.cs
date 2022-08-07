using Microsoft.AspNetCore.Mvc;

namespace _002_WebApiAutores.Controllers.V2
{
    [ApiController]
    //[Route("api/mispruebas")]
    [Route("api/v{version:apiversion}/miscuentas")]
    [ApiVersion("2.0")]
    public class PruebasConfiguracion : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PruebasConfiguracion(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("LecturaConfiguracion")]
        public ActionResult<string> LeeConfiguracion()
        {
            return configuration["MisPruebasConfiguracion"];
            // return configuration["connectionStrings:defaultConnection"];
        }
    }
}
