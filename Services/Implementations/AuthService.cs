using ABMProductos.Models.DTOs.Requests;
using ABMProductos.Repositories.Interfaces;
using ABMProductos.Services.Interfaces;

namespace ABMProductos.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;

        public AuthService(IUserRepository repository)
        {
            _repository = repository;
        }

        public bool ValidateUser(AuthRequestDto authRequest)
        {
            return _repository.IsValidUser(authRequest);
        }
    }
}
