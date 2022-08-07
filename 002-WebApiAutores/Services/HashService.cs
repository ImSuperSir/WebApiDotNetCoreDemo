using _002_WebApiAutores.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace _002_WebApiAutores.Services
{
    public class HashService
    {

        public ResultadoHash Hash(string pTexto)
        {
            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            { 
                random.GetBytes(salt);
            }

            return Hash(pTexto, salt);
        }


        public ResultadoHash Hash(string pTextoPlano, byte[] salt)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(
                password: pTextoPlano, salt: salt, prf: KeyDerivationPrf.HMACSHA256
                , iterationCount: 10000, numBytesRequested: 32);

        
            var hash = Convert.ToBase64String(llaveDerivada);

            return new ResultadoHash()
            { 
                Salt = salt,
                Hash = hash,
            };
        }
    }
}
