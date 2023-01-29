namespace Client.WeatherMvc.Services
{
    public class InteractiveServiceSettings
    {
        public string AuthorityUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string[] Scopes { get; set; }
    }
}
