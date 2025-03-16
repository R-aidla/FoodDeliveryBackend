using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Controllers
{
    /// <summary>
    /// Controls weather based API calls.
    /// </summary>
    [Route("api/weather")]
    [ApiController]
    public class WeatherController(AppDbContext context) : ControllerBase
    {

        // GET: api/weather/latest
        /// <summary>
        /// Get the latest observation stored in the database. Requires auth.
        /// </summary>
        [SecureApi]
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetLatestObservations()
        {
            // Get the most recent timestamp
            var latestTimestamp = await context.WeatherTimestamps
                                                .OrderByDescending(wt => wt.ObservationTime)
                                                .FirstOrDefaultAsync();

            if (latestTimestamp == null)
            {
                return NotFound("No weather data available.");
            }

            // Get the weather observations for the latest timestamp
            var weatherObservations = await context.WeatherObservations
                                                     .Where(w => w.WeatherTimestampId == latestTimestamp.Id)
                                                     .ToListAsync();

            return Ok(new {weatherObservations});
        }

        /// <summary>
        /// Get the closest observation by user give unix time. Requires auth.
        /// </summary>
        [SecureApi]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetObservationsByTime([FromQuery] int? time)
        {
            if(!time.HasValue)
            {
                return NotFound("Can't get valid entry. No time provided.");
            }

            // Get the most recent timestamp
            var latestTimestamp = await context.WeatherTimestamps
                .Where(wt => wt.ObservationTime <= time.Value) // Ensure we are selecting timestamps <= requested time
                .OrderByDescending(wt => wt.ObservationTime) // Order by most recent timestamp first
                .FirstOrDefaultAsync();

            if (latestTimestamp == null)
            {
                return NotFound($"Can't get valid entry. No entry found before {UnixTimeStampToDateTime(time.Value).ToShortDateString()} {UnixTimeStampToDateTime(time.Value).ToShortTimeString()}.");
            }

            // Get the weather observations for the latest timestamp
            var weatherObservations = await context.WeatherObservations
                                                     .Where(w => w.WeatherTimestampId == latestTimestamp.Id)
                                                     .ToListAsync();

            return Ok(new { weatherObservations });
        }

        /// <summary>
        /// Converts 32-bit unix time to DateTime.
        /// </summary>
        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
