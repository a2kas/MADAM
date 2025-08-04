using Tamroutilities.Client;

namespace Tamro.Madam.Application.Clients;

public class MinioHttpClient : ApiHttpClient, IMinioHttpClient
{
    public MinioHttpClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, Constants.MinoClientName, 60)
    {           
    }

    public async Task<string> FetchHtml(string url)
    {
        var result = await GetAsync<string>(url);

        return result.Data;
    }
}
