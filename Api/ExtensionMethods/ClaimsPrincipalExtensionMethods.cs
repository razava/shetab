using IpPanelSmsApi;
using System.Security.Claims;

namespace Api.ExtensionMethods;

public static class ClaimsPrincipalExtensionMethods
{
    public static string? GetUserName(this ClaimsPrincipal value)
    {
        return value.FindFirstValue(ClaimTypes.Name);
    }

    public static string? GetUserId(this ClaimsPrincipal value)
    {
        return value.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
