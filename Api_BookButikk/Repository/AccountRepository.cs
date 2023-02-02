using Api_BookButikk.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public class AccountRepository : IAccountRepository 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _iconfiguration;

        //register repo in seervice configuration/addtransient
        //to sign up, use identitycore's usermanager for application user
        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration iconfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iconfiguration = iconfiguration;
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

        //after jwt configurations
        //created a signin model and injected sing in manager up and before
        public async Task<string> LogIn(SignInModel signInModel) 
        {
            //see parameter 3 and 4.. ispersistent and islockout
            var logIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false) ;

            if (!logIn.Succeeded) 
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                //somehow full qualification needed for jstregistration and therefore prefix added
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_iconfiguration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _iconfiguration["JWT:ValidIssuer"],
                audience: _iconfiguration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //postman get : https://localhost:5001/api/books //receives 401Unauthorized
            //postman post : https://localhost:5001/api/account/login 
            // post body: {"email":"keremmsaga@gmail.com","password":"Test@123."} 
            //receives 200 token, copy token .. then again,
            //postman get : https://localhost:5001/api/books 
                    //get authorization/bearer token/(paste token)/send //gets books list !!!


        }


    }
}
