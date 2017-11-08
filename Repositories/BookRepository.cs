using SHAREit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SHAREit.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Book> FindBySKU(string sku)
        {
            return await _context.book.Where(b => b.sku == sku).FirstOrDefaultAsync();
        }

        async Task<object> IBookRepository.FindUser(int book_id)
        {
            var result = from bookcase in _context.bookcase
                            join user in _context.user on bookcase.user_id equals user.user_id
                            join book in _context.book on bookcase.book_id equals book.book_id 
                            where book.book_id == book_id
                            select new 
                            {
                                user.username,
                                user.address,
                                user.email,
                                user.phone,
                                bookcase.bookcase_id,
                                bookcase.create_time
                            }; 
            return result;
        }

        async Task<object> IBookRepository.FindUser(string sku)
        {
            var result = from bookcase in _context.bookcase
                            join user in _context.user on bookcase.user_id equals user.user_id
                            join book in _context.book on bookcase.book_id equals book.book_id 
                            where book.sku == sku
                            select new 
                            {
                                user.username,
                                user.address,
                                user.email,
                                user.phone,
                                bookcase.bookcase_id,
                                bookcase.create_time
                            }; 
            return result;
        }

        async Task<List<Book>> IBookRepository.Search(string title, int limit, int page)
        {
            return await _context.book.Where(b => b.title.Contains(title))
                                .Take(limit)
                                .Skip((page - 1) * limit)
                                .ToListAsync();
        }
    }
}