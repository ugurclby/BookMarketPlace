using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.WebUI.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace BookMarketPlace.WebUI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor,IOptions<ClientSettings> clientSettings,IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshtoken()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> SignIn(SigninInput signinInput)
        {
            //Token end point e gidilmesi gerekiyor.
            //Identityserver4 discovery end point üzerinden tüm endpointler çekilir.

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }); 

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            // Tüm endpointler çekilde şimdi resource owner credential(password) tipi ile  token almaya gidilir.

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId=_clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Mail,
                Password = signinInput.Password,
                Address=discovery.TokenEndpoint
            };
            
            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
            
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadFromJsonAsync<ErrorDto>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive=true});

                return Response<bool>.Error(responseContent.Errors, 400);

            }

            // Artık token elimde. elimdeki token ile ilgili kullanıcının diğer bilgilerini almam gerekiyor. 

            var userInfoRequest = new UserInfoRequest { Token = token.AccessToken,Address=discovery.UserInfoEndpoint };
            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError) 
            { 
                throw userInfo.Exception;
            }

            // artık elimde kullanıcının tüm bilgileri var. rol gibi. 
            // artık bir cookie oluşturmam gerekiyor.
            // "name","role" parametrelerinin eklememim sebebi controller tarafında HttpContext.User.Identity.Name yaptığımda userinfo içindeki değerin gelmesini sağlamak
            // Cookie'nin kimliğini belirledim.

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,CookieAuthenticationDefaults.AuthenticationScheme,"name","role");
            
            // Cookie'nin temelini oluşturuyorum

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            // Cookie üzerinde access token verefresh token tutucağım

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = signinInput.IsRemember;


            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,authenticationProperties);



            return Response<bool>.Success(true,200); 
        }
    }
}
