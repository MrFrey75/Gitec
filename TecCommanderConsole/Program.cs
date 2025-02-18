using Microsoft.Extensions.DependencyInjection;
using TecCore;
using TecCore.Data;
using Microsoft.EntityFrameworkCore;
using TecCore.Models.Course;
using TecCore.Models.Location;
using TecCore.Models.People;
using TecCore.Services;

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
        
        var graphService = serviceProvider.GetRequiredService<IMsGraphService>();
        
        var users = graphService.GetUsersAsync().Result;
        var groups = graphService.GetGroupsAsync().Result;
        var devices = graphService.GetDevicesAsync().Result;
        
        
        

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
       
        
    }

    // Configure DI services.
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMsGraphService, MsGraphService>();
        services.AddSingleton<IConfigService, ConfigService>();
        services.AddDbContext<TecCoreDbContext>(options =>
            options.UseSqlite($"Data Source={ConfigService.DatabaseFile}"));
    }
}