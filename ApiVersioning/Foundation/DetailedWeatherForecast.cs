namespace ApiVersioning.Foundation
{
    public record DetailedWeatherForecast(int DayNumber, DateTime Date, int TemperatureC, string Summary) : WeatherForecast(Date, TemperatureC, Summary)
    {
        public DetailedWeatherForecast(WeatherForecast forecast, int dayNumber) : this(dayNumber, forecast.Date, forecast.TemperatureC, forecast.Summary)
        {
        }

        public new string Summary => $"Day: {DayNumber}, Weather is {base.Summary}";
    }
}
