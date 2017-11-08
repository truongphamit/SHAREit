using SHAREit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SHAREit.Repositories
{
    public class RateBookcaseRepository : Repository<RateBookcase>, IRateBookcaseRepository
    {
        public RateBookcaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        async Task<object> IRateBookcaseRepository.FindByBookcase(int bookcase_id)
        {
            //return await _context.rate_bookcase.Where(r => r.bookcase_id == bookcase_id).ToListAsync();
            var result = from rate_bookcase in _context.rate_bookcase
                            join user in _context.user on rate_bookcase.user_id equals user.user_id 
                            where rate_bookcase.bookcase_id == bookcase_id
                            select new 
                            {
                                rate_bookcase.rate_bookcase_id,
                                rate_bookcase.bookcase_id,
                                rate_bookcase.create_time, rate_bookcase.star, rate_bookcase.review,
                                user.username
                            }; 
            return result;
        }
    }
}