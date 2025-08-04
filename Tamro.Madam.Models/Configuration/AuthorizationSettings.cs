namespace Tamro.Madam.Models.Configuration;

public class AuthorizationSettings : IAuthorizationSettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Uri { get; set; }
}
