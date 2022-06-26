namespace ApiVersioning.Foundation
{
    public class ForecastProvider
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static string RandomSummary => _summaries[Random.Shared.Next(_summaries.Length)];

        public static WeatherForecast[] GetRandomForecast(int days) =>
            Enumerable.Range(1, days).Select(index => new WeatherForecast(
                Date: DateTime.Now.AddDays(index),
                TemperatureC: Random.Shared.Next(-20, 55),
                Summary: RandomSummary
            ))
            .ToArray();

        public static DetailedWeatherForecast[] GetRandomDetailedForecast(int days) =>
            GetRandomForecast(days)
            .Select(EnrichForecast)
            .ToArray();

        private static DetailedWeatherForecast EnrichForecast(WeatherForecast forecast, int dayNumber) => new(forecast, dayNumber + 1);
    }
}
