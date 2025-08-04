using System.Security.Claims;

namespace Tamro.Madam.Ui.Utils;

public static class ClaimsUtils
{
    public static string[] GetPermissions(this IEnumerable<Claim> claims)
    {
        return claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
    }

    public static string? GetUsername(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(x => x.Type == "sub")?.Value;
    }

    public static string? GetUserName(this IEnumerable<Claim> claims)
    {
        var clientName = claims.FirstOrDefault(x => x.Type == "clientName")?.Value;
        return clientName;
    }
}
