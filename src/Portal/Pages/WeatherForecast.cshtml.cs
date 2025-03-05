using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Portal.Pages
{
    public class WeatherForecastModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IEnumerable<WeatherForecast> WeatherForecasts
        {
            get => GetWeatherForecasts();
        }


        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync()
        {
            IEnumerable<WeatherForecast>? result = null;

            string? baseApiUrl = _configuration.GetValue<string>("Resources:Api");
            string apiUrl = baseApiUrl == null ? string.Empty : baseApiUrl;
            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri(apiUrl);
            httpClient.BaseAddress = uri;

            var requestUri = "api/WeatherForecasts";

            //var requestUri = "weatherforecast";

            var idToken = await HttpContext.GetTokenAsync("id_token");
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var statusCode = response.StatusCode;


            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);
            }
            else
            {
                result = new List<WeatherForecast>();
            }

            return result;
        }

        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            Task<IEnumerable<WeatherForecast>> task = Task.Run<IEnumerable<WeatherForecast>>(async () => await GetWeatherForecastsAsync());
            return task.Result;
        }


        public void OnGet()
        {
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; init; }
    }
}
