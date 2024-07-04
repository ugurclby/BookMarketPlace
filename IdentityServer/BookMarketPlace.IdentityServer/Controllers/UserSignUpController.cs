using BookMarketPlace.IdentityServer.Dtos;
using BookMarketPlace.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using BookMarketPlace.Core.CustomResponse;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using static IdentityServer4.IdentityServerConstants;
namespace BookMarketPlace.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserSignUpController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserSignUpController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpDto userSignUpDto)
        {
            var user =new ApplicationUser { UserName = userSignUpDto.UserName, Email = userSignUpDto.EMail };

            var result= await _userManager.CreateAsync(user, userSignUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(Response<string>.Error(result.Errors.Select(x => x.Description).ToList(), 400));
            }

             return NoContent();
        }

    }
}
