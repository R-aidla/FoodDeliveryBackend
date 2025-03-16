namespace FoodDeliveryBackend
{
    /// <summary>
    /// Helper class that gets any IServiceProvider object.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// The service provider assigned on startup.
        /// </summary>
        public static IServiceProvider? Provider { get; set; }

        /// <summary>
        /// Get a service from a list of services in the Provider requested by type.
        /// </summary>
        public static T GetService<T>() where T : class
        {
            return Provider?.GetService(typeof(T)) as T
                   ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found.");
        }
    }
}
