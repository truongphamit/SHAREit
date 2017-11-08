using SHAREit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SHAREit.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<User> IUserRepository.FindByUsername(string username)
        {
            return await _context.user.Where(u => u.username == username).FirstOrDefaultAsync();
        }
    }
}