namespace Tamro.Madam.Application.Clients;

public interface IMinioHttpClient
{
    Task<string> FetchHtml(string url);
}
