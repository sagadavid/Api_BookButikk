using Microsoft.AspNetCore.Identity;

namespace Api_BookButikk.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
