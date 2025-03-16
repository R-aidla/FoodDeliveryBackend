namespace FoodDeliveryBackend.Models
{
    /// <summary>
    /// The company model.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Company Constructor
        /// </summary>
        public Company() { }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }  // Primary Key

        /// <summary>
        /// The company name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A quick description about the company.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
