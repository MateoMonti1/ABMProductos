using ABMProductos.Models.DTOs.Requests;

namespace ABMProductos.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool IsValidUser(AuthRequestDto dto);
    }
}