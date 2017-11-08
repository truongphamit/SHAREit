using System.Collections.Generic;
using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IRateRepository : IBaseRepository<Rate> 
    {
        Task<object> FindByBook(int book_id);
        Task<bool> isRated(int book_id, int user_id);
    }
}