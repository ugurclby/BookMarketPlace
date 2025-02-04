﻿using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarketPlace.IdentityServer.Services
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";
            private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var request = context.Request.Raw.ToString();

            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "subject_token is required");
                return ;
            }

            var result = await _tokenValidator.ValidateAccessTokenAsync(token);

            if (result.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "subject_token is invalid");
                return;
            }

            var subjectClaim = result.Claims.FirstOrDefault(x=>x.Type=="sub");

            if (subjectClaim == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "subject_token is invalid");
                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value, "access_token",result.Claims);
            
            return;
        }
    }
}
