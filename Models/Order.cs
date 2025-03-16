namespace FoodDeliveryBackend.Models
{
    /// <summary>
    /// The order model.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order Constructor
        /// </summary>
        public Order() { }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Navigation property for EF.
        /// </summary>
        public Company Company { get; set; } = null!;
        
        /// <summary>
        /// The identifier for a company tied to the order.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// With what mode of transport will be used in the delivery?
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Car;

        /// <summary>
        /// Navigation property for EF.
        /// </summary>
        public DeliveryRegionRule? DeliveryRule { get; set; }

        /// <summary>
        /// The identifier for the correct rules.
        /// </summary>
        public int DeliveryRuleId { get; set; }

        /// <summary>
        /// The final cost of the delivery. Currently unused.
        /// </summary>
        public decimal TotalDeliveryCost { get; set; }
    }

    /// <summary>
    /// A enumerator of delivery methods.
    /// </summary>
    public enum DeliveryMethod
    {
        /// <summary> A motor vehicle. </summary>
        Car,

        /// <summary> A bicycle. </summary>
        Bike,

        /// <summary> A motorbike. </summary>
        Scooter
    }
}