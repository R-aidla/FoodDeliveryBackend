namespace FoodDeliveryBackend.Models
{
    /// <summary>
    /// The delivery region rule model.
    /// </summary>
    public class DeliveryRegionRule
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// City/Region Name.
        /// </summary>
        public string? RegionName { get; set; }

        /// <summary>
        /// Base cost for car delivery.
        /// </summary>
        public decimal BaseCarCost { get; set; }

        /// <summary>
        /// Base cost for bike delivery.
        /// </summary>
        public decimal BaseBikeCost { get; set; }

        /// <summary>
        /// Base cost for scooter delivery.
        /// </summary>
        public decimal BaseScooterCost { get; set; }

        /// <summary>
        /// The cost of having snowy weather.
        /// </summary>
        public decimal SnowyWeatherCost { get; set; }

        /// <summary>
        /// The cost of having rainy weather.
        /// </summary>
        public decimal RainyWeatherCost { get; set; }

        /// <summary>
        /// The cost of very low tempetature deliveries.
        /// </summary>
        public decimal MaxLowTemperatureCost { get; set; }

        /// <summary>
        /// The cost of low temperature deliveries.
        /// </summary>
        public decimal MinLowTemperatureCost { get; set; }

        /// <summary>
        /// The cost of high winds.
        /// </summary>
        public decimal HighWindsCost { get; set; }
    }
}
