using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHAREit.Models;

namespace SHAREit.Repositories
{
    public class BorrowRepository : Repository<Borrow>, IBorrowRepository
    {
        public BorrowRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<Borrow> IBorrowRepository.FindByBookcase(int bookcase_id)
        {
            return await _context.borrow.Where(b => b.bookcase_id == bookcase_id).FirstOrDefaultAsync();
        }

        async Task<object> IBorrowRepository.GetBorrowBooks(int user_id)
        {
            var result = from borrow in _context.borrow
                            join bookcase in _context.bookcase on borrow.bookcase_id equals bookcase.bookcase_id
                            join book in _context.book on bookcase.book_id equals book.book_id
                            join user in _context.user on bookcase.user_id equals user.user_id 
                            where bookcase.user_id == user_id
                            select new 
                            {
                                user.username,
                                user.address,
                                user.email,
                                user.phone,
                                bookcase.bookcase_id,
                                book.title,
                                book.author
                            }; 
            return result;
        }

        async Task<object> IBorrowRepository.GetLoanBooks(int user_id)
        {
            var result = from borrow in _context.borrow
                            join bookcase in _context.bookcase on borrow.bookcase_id equals bookcase.bookcase_id
                            join book in _context.book on bookcase.book_id equals book.book_id
                            join user in _context.user on bookcase.user_id equals user.user_id 
                            where bookcase.user_id == user_id && user.user_id == borrow.user_id_borrow
                            select new 
                            {
                                user.username,
                                user.address,
                                user.email,
                                user.phone,
                                bookcase.bookcase_id,
                                book.title,
                                book.author
                            }; 
            return result;
        }
    }
}