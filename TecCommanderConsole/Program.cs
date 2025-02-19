using Microsoft.Extensions.DependencyInjection;
using TecCore;
using TecCore.Data;
using Microsoft.EntityFrameworkCore;
using TecCore.Models.Course;
using TecCore.Models.Location;
using TecCore.Models.People;
using TecCore.Services;
using TecCore.Utilities;

namespace TecCommanderConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        

        using var serviceProvider = services.BuildServiceProvider();
        
        var dbContext = serviceProvider.GetRequiredService<TecCoreDbContext>();

        dbContext.Database.EnsureCreated();
        
        var configService = serviceProvider.GetRequiredService<IConfigService>();
        var graphService = serviceProvider.GetRequiredService<MsGraphService>();

        //RunPredefinedTests();
        
        var u1 = graphService.GetUsersByLastNameAsync("Frey").Result;
        var d1 = graphService.GetAllDevicesAsync().Result;
        var g1 = graphService.GetAllGroupsAsync().Result;

        foreach (var d in d1)
        {
            DeviceHelper.ValidateAndParse(d.DisplayName);
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
       
        
    }

    // Configure DI services.
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MsGraphService>();
        services.AddSingleton<IConfigService, ConfigService>();
        services.AddDbContext<TecCoreDbContext>(options =>
            options.UseSqlite($"Data Source={ConfigService.DatabaseFile}"));
    }
    

}