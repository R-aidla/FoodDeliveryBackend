namespace FoodDeliveryBackend.Models
{
    /// <summary>
    /// The weather observation model.
    /// </summary>
    public class WeatherObservation
    {
        /// <summary>
        /// WeatherObservation Constructor
        /// </summary>
        public WeatherObservation() { }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }  // Primary Key
        
        /// <summary>
        /// The station location and name.
        /// </summary>
        public string StationName { get; set; } = string.Empty;

        /// <summary>
        /// The station's code.
        /// </summary>
        public string WmoCode { get; set; } = string.Empty;

        /// <summary>
        /// The temperature reading in the station.
        /// </summary>
        public float AirTemperature { get; set; }

        /// <summary>
        /// The wind speeds at the station.
        /// </summary>
        public float WindSpeed { get; set; }

        /// <summary>
        /// The current weather from the station.
        /// </summary>
        public string WeatherPhenomenon { get; set; } = string.Empty;

        /// <summary>
        /// The identifier for the timestamp of the station readings.
        /// </summary>
        public int? WeatherTimestampId { get; set; }

        /// <summary>
        /// Navigation property for EF.
        /// </summary>
        public WeatherTimestamp? WeatherTimestamp { get; set; }
    }
}
