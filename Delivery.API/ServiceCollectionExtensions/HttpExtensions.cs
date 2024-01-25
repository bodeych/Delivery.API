using System.Security.Claims;

namespace Delivery.API.ServiceCollectionExtensions;

public static class HttpExtensions
{
        public static Guid GetIdUser(this HttpContext httpContext)
        {
            if (httpContext.User is null)
                return Guid.Empty;

            var claim = httpContext.User.Claims.Single(x => x.Type == "creatorId");

            return Guid.Parse(claim.Value);
        }
}