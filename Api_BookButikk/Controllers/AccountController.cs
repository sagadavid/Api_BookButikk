using Api_BookButikk.Model;
using Api_BookButikk.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api_BookButikk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        { 
        var signingUp = await _accountRepository.SignUp(signUpModel);
            if (signingUp.Succeeded) 
                   { return Ok(signingUp.Succeeded); }
           
            return Unauthorized();
        }


    }
}
