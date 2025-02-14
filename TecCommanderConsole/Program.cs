using Microsoft.Extensions.DependencyInjection;
using TecCore;

namespace TecCommanderConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();

        var configService = serviceProvider.GetRequiredService<ConfigService>();

        Console.WriteLine($"App Name: {configService.Config.AppCoreName}");
        Console.WriteLine($"App Version: {configService.Config.AppCoreVersion}");

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    // Configure DI services.
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ConfigService>();
    }
}