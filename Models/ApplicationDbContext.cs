using Microsoft.EntityFrameworkCore;

namespace SHAREit.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt)
            : base(opt)
        {
        }
        public DbSet<Book> book { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Bookcase> bookcase {get; set;}
        public DbSet<RateBookcase> rate_bookcase {get; set;}
        public DbSet<Rate> rate_book {get; set;}
         public DbSet<Borrow> borrow {get; set;}
    }
}