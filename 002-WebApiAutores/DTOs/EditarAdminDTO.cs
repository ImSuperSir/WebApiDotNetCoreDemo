using System.ComponentModel.DataAnnotations;

namespace _002_WebApiAutores.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
