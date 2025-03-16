namespace FoodDeliveryBackend.Models
{
    /// <summary>
    /// The weather timestamp model.
    /// </summary>
    public class WeatherTimestamp
    {

        /// <summary>
        /// WeatherTimestamp Constructor
        /// </summary>
        public WeatherTimestamp() { }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The last observed time.
        /// </summary>
        public long ObservationTime { get; set; }
    }
}