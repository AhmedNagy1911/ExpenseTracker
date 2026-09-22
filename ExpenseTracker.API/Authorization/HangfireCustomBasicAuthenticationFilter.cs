using Hangfire.Dashboard;
using System.Text;

namespace ExpenseTracker.API.Authorization;

public class HangfireCustomBasicAuthenticationFilter : IDashboardAuthorizationFilter
{
    public string? User { get; set; }
    public string? Pass { get; set; }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var header = httpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(header) || !header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            httpContext.Response.Headers.WWWAuthenticate = "Basic realm=\"Hangfire Dashboard\"";
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }

        var encoded = header["Basic ".Length..].Trim();
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        var parts = decoded.Split(':', 2);

        if (parts.Length != 2)
            return false;

        return parts[0] == User && parts[1] == Pass;
    }
}
