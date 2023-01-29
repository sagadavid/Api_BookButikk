using Microsoft.EntityFrameworkCore;

namespace Api_BookButikk.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext
            (DbContextOptions <BookStoreDbContext> contextOptions)
            :base(contextOptions)
        {
        }

        public DbSet<Books> DbSBooks { get; set; }

        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;BookButikkAPI;Integrated Security=True");
            base.OnConfiguring (optionsBuilder);
        }

    }
}
