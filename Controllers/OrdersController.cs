using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;
using System.Text.RegularExpressions;
using FoodDeliveryBackend.Models.DTO;

namespace FoodDeliveryBackend.Controllers
{
    /// <summary>
    /// Controls order based API calls.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(AppDbContext context) : ControllerBase
    {
        // POST: api/orders/calculate
        /// <summary>
        /// Calculate the cost of the delivery using the values given by the user.
        /// </summary>
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateOrderCost([FromBody] OrderDTO order)
        {
            if (order == null)
                return BadRequest("Invalid order data.");

            var deliveryRegion = await context.DeliveryRegionRules.FindAsync(order.DestinationRegionId);

            var latestTimestamp = await context.WeatherTimestamps
                .OrderBy(wt => wt.ObservationTime <= order.DeliveryTimePeriod)
                .FirstOrDefaultAsync();

            WeatherObservation? weatherObservation = null;

            if (latestTimestamp != null && deliveryRegion != null)
            {
                weatherObservation = await context.WeatherObservations
                    .Where(w => w.WeatherTimestampId == latestTimestamp.Id &&
                    EF.Functions.Like(w.StationName, $"%{deliveryRegion.RegionName}%"))
                    .FirstAsync();
            }

            if (weatherObservation == null)
            {
                return BadRequest("Invalid locations or unknown weather observation data.");
            }

            decimal weatherCost = 0m;
            decimal temperatureCost = 0m;
            decimal windCost = 0m;

#pragma warning disable CS8602 // Dereference of a possibly null reference. I've added a simple null check above already.
            decimal baseCost = order.DeliveryMethod switch
            {
                DeliveryMethod.Car => deliveryRegion.BaseCarCost,
                DeliveryMethod.Scooter => deliveryRegion.BaseScooterCost,
                DeliveryMethod.Bike => deliveryRegion.BaseBikeCost,
                _ => deliveryRegion.BaseCarCost
            };
#pragma warning restore CS8602 

            var weatherCondition = weatherObservation.WeatherPhenomenon;
            var windSpeed = weatherObservation.WindSpeed;
            var temperature = weatherObservation.AirTemperature; 

            var weatherSnowPattern = @"\b(snow|snowfall)\b";
            var weatherRainPattern = @"\b(rain)\b";
            var badWeatherPattern = @"\b(glaze|hail|thunder)\b";

            if (order.DeliveryMethod == DeliveryMethod.Bike || order.DeliveryMethod == DeliveryMethod.Scooter)
            {
                if (Regex.IsMatch(weatherCondition, weatherSnowPattern, RegexOptions.IgnoreCase))
                    weatherCost = deliveryRegion.SnowyWeatherCost;

                else if (Regex.IsMatch(weatherCondition, weatherRainPattern, RegexOptions.IgnoreCase))
                    weatherCost = deliveryRegion.RainyWeatherCost;

                else if (Regex.IsMatch(weatherCondition, badWeatherPattern, RegexOptions.IgnoreCase))
                    return Conflict("Usage of selected vehicle type is forbidden");


                if (windSpeed < 20f && windSpeed >= 10f)
                    windCost = deliveryRegion.HighWindsCost;

                else if (windSpeed >= 20f)
                    return Conflict("Usage of selected vehicle type is forbidden");


                if (temperature > -10 && temperature <= 0)
                    temperatureCost = deliveryRegion.MinLowTemperatureCost;
                else if (temperature <= -10)
                    temperatureCost = deliveryRegion.MaxLowTemperatureCost;
            }

            decimal totalDeliveryCost = baseCost + temperatureCost + windCost + weatherCost;

            return Ok(new { totalDeliveryCost });
        }
    }
}
