namespace FoodDeliveryBackend.Models.DTO
{
    /// <summary>
    /// The order data transfer object model. Used in API calls for orders.
    /// </summary>
    public class OrderDTO
    {
        /// <summary>
        /// The identifier for a company tied to the order.
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// The identifier for the correct region rule.
        /// </summary>
        public int DestinationRegionId { get; set; }

        /// <summary>
        /// With what mode of transport will be used in the delivery?
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Car;

        /// <summary>
        /// At what time period has the delivery happened?
        /// </summary>
        public long DeliveryTimePeriod { get; set; }
    }
}
