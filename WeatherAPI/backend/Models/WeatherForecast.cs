namespace backend.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double WindSpeed { get; set; }
    }
}
