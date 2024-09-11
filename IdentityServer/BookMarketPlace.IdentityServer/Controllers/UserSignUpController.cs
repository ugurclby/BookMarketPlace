using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.IdentityServer.Dtos;
using BookMarketPlace.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;
namespace BookMarketPlace.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/{action}")]
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
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
             var userIdClaim = User.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Sub );
            if (userIdClaim == null) return BadRequest();
            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null) return BadRequest();
            return Ok(new {Id=user.Id,
                UserName=user.UserName,
                Email=user.Email
            });
        }
    }
}
