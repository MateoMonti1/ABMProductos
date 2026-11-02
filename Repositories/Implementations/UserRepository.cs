using ABMProductos.Data;
using ABMProductos.Entities;
using ABMProductos.Models.DTOs.Requests;
using ABMProductos.Repositories.Interfaces;

namespace ABMProductos.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ABMProductosContext _context;

        public UserRepository(ABMProductosContext context)
        {
            _context = context;
        }

        public bool IsValidUser(AuthRequestDto dto)
        {
            //FirstOrDefault
            //any
            //Single
            User? result = _context.Users.SingleOrDefault(u => u.Password.Equals(dto.Password) && u.Email.Equals(dto.Email));
            return result != null;
        }
    }
}
