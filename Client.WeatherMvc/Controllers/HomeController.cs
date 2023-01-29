using Client.WeatherMvc.Models;
using Client.WeatherMvc.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Client.WeatherMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Weather()
        {
            using var client = new HttpClient();

            var token = await HttpContext.GetTokenAsync("access_token");
            //var token = await _tokenService.GetToken("weatherapi.read weatherapi.write");
            client.SetBearerToken(token);

            var result = await client.GetAsync("https://localhost:5445/weatherforecast");

            if (result.IsSuccessStatusCode)
            {
                var weather = JsonConvert.DeserializeObject<IEnumerable<WeatherData>>(
                    await result.Content.ReadAsStringAsync());

                return View(weather);
            }

            throw new Exception("Unable to get content");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}