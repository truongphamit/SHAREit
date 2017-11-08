using System.Collections.Generic;
using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IBookRepository : IBaseRepository<Book> 
    {
        Task<Book> FindBySKU(string sku);
        Task<object> FindUser(int book_id);
        Task<object> FindUser(string sku);
        Task<List<Book>> Search(string title, int limit, int page);
    }
}