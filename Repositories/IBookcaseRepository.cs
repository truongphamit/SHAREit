using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IBookcaseRepository : IBaseRepository<Bookcase>
    {
        Task<object> FindByUsername(string username);
    }
}