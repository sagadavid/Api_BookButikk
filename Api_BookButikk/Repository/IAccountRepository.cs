using Api_BookButikk.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUp(SignUpModel signUpModel);
    }
}
