using backend.Services;
using System.Threading.Tasks;
namespace backend.Jobs
{
    public class WeatherFetchJob
    {
        private readonly WeatherService _weatherService;

        public WeatherFetchJob(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task ExecuteAsync()
        {
            await _weatherService.FetchAndStoreWeatherData();
        }
    }
}
