using System.ComponentModel.DataAnnotations;

namespace ABMProductos.Models.DTOs.Requests
{
    public class AuthRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
