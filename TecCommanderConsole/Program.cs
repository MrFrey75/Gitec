using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Services;
using Microsoft.Extensions.DependencyInjection;
using TecCore;
using TecCore.Data;
using Microsoft.EntityFrameworkCore;
using TecCore.Services;
using TecCore.Services.Entra;
using TecCore.Services.Google;
using TecCore.Utilities;

namespace TecCommanderConsole;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        

        using var serviceProvider = services.BuildServiceProvider();
        
        var dbContext = serviceProvider.GetRequiredService<TecCoreDbContext>();

        dbContext.Database.EnsureCreated();
        
        var configService = serviceProvider.GetRequiredService<IConfigService>();
        
        var entraUserService = serviceProvider.GetRequiredService<TecCore.Services.Entra.UserService>();
        var entraGroupService = serviceProvider.GetRequiredService<TecCore.Services.Entra.GroupService>();
        var entraDeviceService = serviceProvider.GetRequiredService<TecCore.Services.Entra.DeviceService>();
        
        
        var googleService = serviceProvider.GetRequiredService<TecCore.Services.Google.UserService>();
        var orgUnitService = serviceProvider.GetRequiredService<TecCore.Services.Google.OrgUnitService>();
        
        //RunPredefinedTests();
        
        var entraUsers = await entraUserService.GetAllStaffAsync();
        var entraGroups = await entraGroupService.GetAllGroupsAsync();
        var entraDevices = await entraDeviceService.GetAllDevicesAsync();
        
        var googleUsers = await googleService.GetAllUsersAsync();
        var googleOrgUnits = await orgUnitService.GetAllOrgUnitsAsync();
        var mergeService = serviceProvider.GetRequiredService<PeopleMergeService>();
        

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
       
        
    }

    // Configure DI services.
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<TecCore.Services.Google.UserService>();
        services.AddSingleton<TecCore.Services.Google.OrgUnitService>();
        
        services.AddSingleton<TecCore.Services.Entra.DeviceService>();
        services.AddSingleton<TecCore.Services.Entra.GroupService>();
        services.AddSingleton<TecCore.Services.Entra.UserService>();
        services.AddSingleton<TecCore.Services.PeopleMergeService>();

        
        
        services.AddSingleton<IDirectoryServiceFactory, DirectoryServiceFactory>();
        services.AddSingleton<IConfigService, ConfigService>();
        
        services.AddSingleton<DirectoryService>(provider =>
        {
            var credential = GoogleHelper.GetCredential().CreateWithUser("af@gi-tec.net");
            return new DirectoryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GITEC Admin API"
            });
        });
        
        
        services.AddDbContext<TecCoreDbContext>(options =>
            options.UseSqlite($"Data Source={ConfigService.DatabaseFile}"));
    }
    

}