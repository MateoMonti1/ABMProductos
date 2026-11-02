using ABMProductos.Models.DTOs.Requests;

namespace ABMProductos.Services.Interfaces
{
    public interface IAuthService
    {
        public bool ValidateUser(AuthRequestDto authRequest);
    }
}