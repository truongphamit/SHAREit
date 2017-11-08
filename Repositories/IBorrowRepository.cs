using System.Threading.Tasks;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public interface IBorrowRepository : IBaseRepository<Borrow>
    {
        Task<Borrow> FindByBookcase(int bookcase_id);

        Task<object> GetBorrowBooks(int user_id);
        Task<object> GetLoanBooks(int user_id);
    }
}