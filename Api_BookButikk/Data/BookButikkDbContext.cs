using Microsoft.EntityFrameworkCore;

namespace Api_BookButikk.Data
{
    public class BookButikkDbContext : DbContext
    {
        public BookButikkDbContext
            (DbContextOptions <BookButikkDbContext> contextOptions)
            :base(contextOptions)
        {
        }

        public DbSet<Books> Books { get; set; }//we could take bookmodel, instead

        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            ////connection string defined by onconfiguring, an alternative is to degine in startup/services
            //optionsBuilder.UseSqlServer("Server=.;Database=BookButikkAPI;Integrated Security=True");
            //base.OnConfiguring (optionsBuilder);
        }

    }
}
