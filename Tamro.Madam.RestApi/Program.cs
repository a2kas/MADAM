using TamroUtilities.Exceptions;

namespace Tamro.Madam.RestApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.SetupPipeline();

        app.Run();
    }
}
