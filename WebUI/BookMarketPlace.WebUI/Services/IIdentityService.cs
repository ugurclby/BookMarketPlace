using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.WebUI.Models;
using IdentityModel.Client;

namespace BookMarketPlace.WebUI.Services
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshtoken(); 
    }
}
