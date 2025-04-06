using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;
using Microsoft.Extensions.Logging;
namespace backend.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherDbContext _dbContext;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, WeatherDbContext dbContext, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task FetchAndStoreWeatherData()
        {
            try
            {
                string url = "https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch weather data: {response.StatusCode}");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var hourly = root.GetProperty("hourly");
                var timestamps = hourly.GetProperty("time").EnumerateArray().Select(x => DateTime.Parse(x.GetString())).ToList();
                var temperatures = hourly.GetProperty("temperature_2m").EnumerateArray().Select(x => x.GetDouble()).ToList();
                var humidities = hourly.GetProperty("relative_humidity_2m").EnumerateArray().Select(x => x.GetDouble()).ToList();
                var windSpeeds = hourly.GetProperty("wind_speed_10m").EnumerateArray().Select(x => x.GetDouble()).ToList();

                List<WeatherForecast> weatherRecords = new List<WeatherForecast>();

                for (int i = 0; i < timestamps.Count; i++)
                {
                    weatherRecords.Add(new WeatherForecast
                    {
                        Date = DateTime.SpecifyKind(timestamps[i], DateTimeKind.Utc),
                        Temperature = temperatures[i],
                        Humidity = humidities[i],
                        WindSpeed = windSpeeds[i]
                    });
                }


                await _dbContext.WeatherRecords.AddRangeAsync(weatherRecords);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Weather data saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching and storing weather data");
            }
        }
    }
}
