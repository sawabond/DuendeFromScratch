using IdentityModel.Client;

namespace Client.WeatherMvc.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}