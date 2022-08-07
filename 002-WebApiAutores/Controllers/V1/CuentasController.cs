using _002_WebApiAutores.DTOs;
using _002_WebApiAutores.Services;
using ImSuperSir.BankingServices.Common.DTOs;
using ImSuperSir.BankingServices.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _002_WebApiAutores.Controllers.V1
{
    [ApiController]
    //[Route("Cuentas")]
    [Route("api/v{version:apiversion}/cuentas")]
    [ApiVersion("1.0")]
    public class CuentasController : ControllerBase
    {
        private readonly IBankingSecurityService bankingSecurityService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector; // para la proteccion de los datos

        public CuentasController(IBankingSecurityService bankingSecurityService
            , UserManager<IdentityUser> userManager
            , IDataProtectionProvider dataProtectionProvider
            , HashService hashService)
        {
            this.bankingSecurityService = bankingSecurityService;
            this.userManager = userManager;
            this.hashService = hashService;

            dataProtector = dataProtectionProvider.CreateProtector("valorUnicoyObligatoriamenteSecreto"); // para la proteccion de los datos
        }


        [HttpGet("hash/{textoPlano}")]
        public ActionResult RealizarHash(string textoPlano)
        {
            var lResultado = hashService.Hash(textoPlano);
            var lResultado2 = hashService.Hash(textoPlano);

            return Ok(
                new
                {
                    Resultado1 = lResultado,
                    Resultado2 = lResultado2
                }
                );
        }


        [HttpGet("encriptar")]
        public ActionResult Encriptar()
        {
            string lTexto = "Lauro Ramirez";
            string lTextoEncriptado = dataProtector.Protect(lTexto);
            string lTextoDesencriptado = dataProtector.Unprotect(lTextoEncriptado);

            return Ok(new
            {
                Texto = lTexto,
                TextoEncriptado = lTextoEncriptado,
                TextoDesencripado = lTextoDesencriptado
            });


        }


        [HttpGet("encriptarPorTiempo")]
        public ActionResult EncriptarPorTiempo()
        {

            var lDataProtector = dataProtector.ToTimeLimitedDataProtector();

            string lTexto = "Lauro Ramirez, TimeSpan.";
            string lTextoEncriptado = lDataProtector.Protect(lTexto, TimeSpan.FromSeconds(6));
            string lTextoDesencriptado = lDataProtector.Unprotect(lTextoEncriptado);

            return Ok(new
            {
                Texto = lTexto,
                TextoEncriptado = lTextoDesencriptado,
                TextoDesencripado = lTextoDesencriptado
            });
        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponse>> Renovar()
        {
            var lEmailClient = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var lEmail = lEmailClient.Value;
            var lCredencialesUsuario = new UserCredentials()
            {
                Email = lEmail
            };

            return await bankingSecurityService.ContruirTokenRespuesta(lCredencialesUsuario);

        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var lUser = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(lUser, new Claim("esAdmin", "1"));
            return NoContent();

        }


        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var lUser = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimsAsync(lUser, new List<Claim>() { new Claim("esAdmin", "1") });
            return NoContent();

        }


    }
}
