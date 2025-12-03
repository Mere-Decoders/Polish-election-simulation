namespace backend.Data
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
