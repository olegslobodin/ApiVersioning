namespace ApiVersioning.Foundation
{
    public record WeatherForecast(DateTime Date, int TemperatureC, string Summary)
    {
        /// <summary>
        /// Temperature in Fahrenheits for some weird guys which do not use metric system
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
