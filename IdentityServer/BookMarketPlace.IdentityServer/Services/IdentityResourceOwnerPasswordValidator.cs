using BookMarketPlace.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMarketPlace.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existsUser = await _userManager.FindByEmailAsync(context.UserName);

            if (existsUser == null)
            {
                var dictionary = new Dictionary<string, object>();
                dictionary.Add("errors",new List<string>() { "Email ya da şifre yanlış"});

                context.Result.CustomResponse = dictionary;
                return;
            }

            var checkPass= await _userManager.CheckPasswordAsync(existsUser, context.Password);

            if (!checkPass)
            {
                var dictionary = new Dictionary<string, object>();
                dictionary.Add("errors", new List<string>() { "Email ya da şifre yanlış" });

                context.Result.CustomResponse = dictionary;
                return;
            }

            context.Result = new GrantValidationResult(existsUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
