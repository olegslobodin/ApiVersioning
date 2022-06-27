using ApiVersioning.Foundation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")] //Actual version (is default)
    [ApiVersion("3.0")] //Beta version
    [Route("api/[controller]")] //Version must be specified in Header, Media type or Query string
    [Route("api/v{version:apiVersion}/[controller]")]   //Duplicate endpoints with ability to specify version in route
    [Produces("application/json")]
    [EnableCors(Constants.Policies.AllowOrigin.Any)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [DisableCors]   //CORS disabled
        public IReadOnlyCollection<WeatherForecast> Get10() => ForecastProvider.GetRandomForecast(5);

        /// <summary>
        /// Get weather forecast
        /// </summary>
        /// <param name="days">Days to forecast</param>
        /// <remarks>
        /// Current API version is v2. You may try beta v3 request by explicitly specifyning version 3
        ///
        ///     v3 sample response:
        ///     [
        ///       {
        ///         "dayNumber": 1,
        ///         "summary": "Day: 1, Weather is Bracing",
        ///         "date": "2022-06-27T11:53:47.7895543+03:00",
        ///         "temperatureC": 9,
        ///         "temperatureF": 48
        ///       }
        ///     ]
        ///
        /// </remarks>
        /// <response code="400">If days count is less than <see cref="Constants.Forecasts.Params.MinDaysCount"/></response>
        [HttpGet]
        [MapToApiVersion("2.0")]
        [DisableCors]   //CORS enabled only for Google
        [EnableCors(Constants.Policies.AllowOrigin.Google)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(string))]
        public ActionResult<IReadOnlyCollection<WeatherForecast>> Get20(int days) =>
            ValidateForecastDays(days, out var error)
            ? ForecastProvider.GetRandomForecast(days)
            : BadRequest(error);

        [HttpGet]       //CORS enabled for any origin
        [MapToApiVersion("3.0")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(string))]
        public ActionResult<IReadOnlyCollection<DetailedWeatherForecast>> Get30(int days) =>
            ValidateForecastDays(days, out var error)
            ? ForecastProvider.GetRandomDetailedForecast(days)
            : BadRequest(error);

        /// <remarks>
        /// Is removed in v3
        /// </remarks>
        [HttpGet("current")]
        [Obsolete("'current' endpoint is removed in v3")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [DisableCors]
        public WeatherForecast GetCurrent() => new(DateTime.Now, 25, "Warm");


        private static bool ValidateForecastDays(int days, out string errorMessage)
        {
            var isValid = days >= Constants.Forecasts.Params.MinDaysCount;
            errorMessage = isValid ? String.Empty : $"Specify at least {Constants.Forecasts.Params.MinDaysCount} day(s)";

            return isValid;
        }
    }
}