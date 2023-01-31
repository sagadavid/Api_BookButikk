using Api_BookButikk.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api_BookButikk.Data
{
    //public class BookButikkDbContext : DbContext

    //since we use separate identity class, we use identitydbcontext
    //which is also derived from dbcontext and identiy user
    
    public class BookButikkDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookButikkDbContext
            (DbContextOptions<BookButikkDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<Books> Books { get; set; }//we could take bookmodel, instead

        //protected override void OnConfiguring
        //    (DbContextOptionsBuilder optionsBuilder)
        //{
        //    ////connection string defined by onconfiguring, an alternative is to degine in startup/services
        //    //optionsBuilder.UseSqlServer("Server=.;Database=BookButikkAPI;Integrated Security=True");
        //    //base.OnConfiguring (optionsBuilder);
        //}

    }
}
