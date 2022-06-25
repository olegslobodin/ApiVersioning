using ApiVersioning.Foundation;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IReadOnlyCollection<WeatherForecast> Get10() => ForecastProvider.GetRandomForecast(5);

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IReadOnlyCollection<WeatherForecast> Get20(int days) => ForecastProvider.GetRandomForecast(days);

        [HttpGet]
        [MapToApiVersion("3.0")]
        public IReadOnlyCollection<DetailedWeatherForecast> Get30(int days) => ForecastProvider.GetRandomDetailedForecast(days);
    }
}