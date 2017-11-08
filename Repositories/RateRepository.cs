using SHAREit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SHAREit.Repositories
{
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<object> IRateRepository.FindByBook(int book_id)
        {
            //return await _context.rate_bookcase.Where(r => r.bookcase_id == bookcase_id).ToListAsync();
            var result = from rate_book in _context.rate_book
                            join user in _context.user on rate_book.user_id equals user.user_id 
                            where rate_book.book_id == book_id
                            select new 
                            {
                                rate_book.rate_id,
                                rate_book.book_id,
                                rate_book.create_time, 
                                rate_book.star, 
                                rate_book.review,
                                user.username
                            }; 
            return result;
        }

        async Task<bool> IRateRepository.isRated(int book_id, int user_id)
        {
            var result = from rate_book in _context.rate_book
                    where rate_book.book_id == book_id && rate_book.user_id == user_id
                    select new {

                    };

            return result.Count() != 0;
        }
    }
}