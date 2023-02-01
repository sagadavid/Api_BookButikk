using Api_BookButikk.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public class AccountRepository : IAccountRepository 
    {
        private readonly UserManager<ApplicationUser> _userManager;

        //register repo in seervice configuration/addtransient
        //to sign up, use identitycore's usermanager for application user
        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> SignUp(SignUpModel signUpModel)//pass this to iaccountrepo also
        {
            //add new user by usermanager
            var user = new ApplicationUser()
            {
                //all properties come from identityuser
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                Email = signUpModel.Email,
                UserName = signUpModel.Email
            };
            //now create this user by password
            return await _userManager.CreateAsync(user, signUpModel.Password);
            
            /*postman post: https://localhost:5001/api/account/signup
             {
"firstname": "david",
"lastname":"saga",
"email":"davidsagaepost@gmail.com",
"password":"Test@123.",
"confirmpassword":"Test@123."
}
            */

        }
    }
}
