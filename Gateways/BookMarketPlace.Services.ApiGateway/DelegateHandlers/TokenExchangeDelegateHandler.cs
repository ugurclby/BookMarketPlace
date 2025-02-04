
using IdentityModel.Client;

namespace BookMarketPlace.Services.ApiGateway.DelegateHandlers;

public class TokenExchangeDelegateHandler : DelegatingHandler
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string _accessToken;
    public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private async Task<string> GetToken(string requestToken)
    {
        if (!string.IsNullOrEmpty(_accessToken))
        {
            return _accessToken;
        }

        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _configuration.GetSection("IdentityServerUrl").Value,
            Policy = new DiscoveryPolicy
            {
                RequireHttps = false
            }
        });
        if (disco == null)
        {
            throw disco.Exception;
        }

        TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = _configuration["ClientId"],
            ClientSecret = _configuration["ClientSecret"],
            GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
            SubjectToken = requestToken,
            SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token",
            Scope = "discount_fullperms fakePayment_fullperms openid"
        };

        TokenResponse tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);

        if (tokenResponse.IsError)
        {
            throw tokenResponse.Exception;
        }

        _accessToken = tokenResponse.AccessToken;

        return _accessToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestToken = request.Headers.Authorization?.Parameter;

        if (!string.IsNullOrEmpty(requestToken))
        {
            var token = await GetToken(requestToken);
            request.SetBearerToken(token);
         }

        return await base.SendAsync(request, cancellationToken);
    }
}
