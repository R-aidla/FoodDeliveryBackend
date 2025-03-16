using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodDeliveryBackend
{
    /// <summary>
    /// An attribute that forces the user to be authorized with a cookie
    /// containing the Admin Key when calling a ApiController call.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SecureApiAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _secretKey;

        /// <summary>
        /// Forces the user to be authorized with a cookie containing key/token.
        /// </summary>
        public SecureApiAttribute()
        {
            // Fetch IConfiguration from ServiceLocator
            var config = ServiceLocator.GetService<IConfiguration>();

            // Retrieve the value from appsettings.json
            _secretKey = config["Secrets:AdminKey"] ?? throw new Exception("Secret Key is missing!");
        }

        /// <summary>
        /// Checks the users cookie data, get and checks the secret key saved.
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool doesHeaderContainKey = context.HttpContext.Request.Headers.TryGetValue("X-API-Key", out var apiKeyHeader);
            bool doesUserHaveKeyInCookie = context.HttpContext.Request.Cookies.TryGetValue("AdminKey", out var apiKey);

            if ((!doesHeaderContainKey && !doesUserHaveKeyInCookie) || (apiKeyHeader != _secretKey && apiKey != _secretKey))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
