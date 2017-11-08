using System.Threading.Tasks;
using SHAREit.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SHAREit.Repositories
{
    public class BookcaseRepository : Repository<Bookcase>, IBookcaseRepository
    {
        public BookcaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<object> IBookcaseRepository.FindByUsername(string username)
        {
            var result = from bookcase in _context.bookcase
                            join user in _context.user on bookcase.user_id equals user.user_id
                            join book in _context.book on bookcase.book_id equals book.book_id 
                            where user.username == username
                            select new 
                            {
                                user.username,
                                bookcase.bookcase_id,
                                bookcase.create_time,
                                book.sku, 
                                book.book_id, 
                                book.title,
                                book.description,
                                book.sub_description,
                                book.company,
                                book.image,
                                book.author
                            }; 
            return result;
        }
    }
}