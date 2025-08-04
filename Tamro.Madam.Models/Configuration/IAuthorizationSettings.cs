namespace Tamro.Madam.Models.Configuration;

public interface IAuthorizationSettings
{
    string ClientId { get; }
    string ClientSecret { get; }
    string Uri { get; }
}
