using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.WeatherMvc.Services;

public sealed class TokenService : ITokenService
{
    private readonly IdentityServerSettings _identityServerSettings;
    private readonly DiscoveryDocumentResponse _discoveryDocument;
    public TokenService(IOptions<IdentityServerSettings> identityServerSettings)
    {
        _identityServerSettings = identityServerSettings.Value;

        using var httpClient = new HttpClient();

        _discoveryDocument = httpClient.GetDiscoveryDocumentAsync(_identityServerSettings.DiscoveryUrl).Result;
        if (_discoveryDocument.IsError)
        {
            throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
        }
    }
    public async Task<TokenResponse> GetToken(string scope)
    {
        using var httpClient = new HttpClient();
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = _discoveryDocument.TokenEndpoint,

            ClientId = _identityServerSettings.ClientName,
            ClientSecret = _identityServerSettings.ClientPassword,
            Scope = scope
        });

        if (tokenResponse.IsError)
        {
            throw new Exception("Error getting token", tokenResponse.Exception);
        }

        return tokenResponse;
    }
}
