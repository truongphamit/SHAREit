using System.Collections.Generic;
using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IRateBookcaseRepository : IBaseRepository<RateBookcase> 
    {
        Task<object> FindByBookcase(int bookcase_id);
    }
}