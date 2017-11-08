using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IUserRepository : IBaseRepository<User> 
    {
        Task<User> FindByUsername(string username);
    }
}