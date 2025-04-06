using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<WeatherForecast> WeatherRecords { get; set; }
    }
}
