using ABMProductos.Models.DTOs.Requests;
using ABMProductos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABMProductos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("authenticate")]
        public IActionResult CreateSession(AuthRequestDto dto)
        {
            bool result = _service.ValidateUser(dto);
            if (result)
            {
                return Ok("el usuario es válido");
            }
            return Unauthorized("El usuario no está registrado o la contraseña es inválida");
        }
    }
}
