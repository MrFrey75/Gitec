using System.Text;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Services;
using Microsoft.Extensions.DependencyInjection;
using TecCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using TecCore.Services;
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

        await dbContext.Database.EnsureCreatedAsync();
        
        var configService = serviceProvider.GetRequiredService<IConfigService>();
        
        // var entraUserService = serviceProvider.GetRequiredService<TecCore.Services.Entra.UserService>();
        // var googleService = serviceProvider.GetRequiredService<TecCore.Services.Google.UserService>();
        // var orgUnitService = serviceProvider.GetRequiredService<TecCore.Services.Google.OrgUnitService>();
        
        var deviceList = new List<string>
        {
            "CTE503T2301",
            "CTE503S2302",
            "CTE503S2303",
            "CTE503S2304",
            "CTE503S2305",
            "CTE503S2306",
            "CTE503S2307",
            "CTE503S2308",
            "CTE503S2309",
            "CTE503S2310",
            "CTE503S2312",
            "CTE-503-2313",
            "CTE503S2314",
            "CTE503S2315",
            "CTE503S2316",
            "CTE-503-V2317",
            "CTE503S2318",
            "CTE-503-V2319",
            "CTE-503-V2320",
            "CTE-503-V2321",
            "CTE-503-V2322",
            "CTE-503-V2323",
            "CTE-503-V2324",
            "CTE-503-V2325",
            "CTE-503-V2326",
            "CTE-503-V2327",
            "CTE-503-V2328",
            "CTE-503-V2329",
            "CTE503S2311",
            "CTE-511-V2330",
            "CTE-511-V2335",
            "CTE-511-V2345",
            "CTE-511-V2332",
            "CTE-511-V2331",
            "CTE-511-V2337",
            "CTE-511-V2338",
            "CTE-511-V2336",
            "CTE-511-V2339",
            "CTE-511-V2341",
            "CTE-511-V2342",
            "CTE-511-V2343",
            "CTE-511-V2344",
            "CTE-511-V2334",
            "CTE-511-V2346",
            "CTE-511-V2347",
            "CTE-511-V2348",
            "CTE-511-V2349",
            "CTE-511-V2350",
            "CTE-511-V2351",
            "CTE-511-V2352",
            "CTE-511-V2353",
            "CTE-511-V2354",
            "CTE-511-V2356",
            "CTE-511-V2355",
            "CTE-511-V2357",
            "CTE-511-V2340",
            "CTE-511-V2359",
            "CTE-511-V2360",
            "CTE-601-V2657",
            "CTE-601-V2176",
            "CTE-601-V2499",
            "CTE-601-V2501",
            "CTE-601-V2500",
            "CTE-601-V2548",
            "CTE-601-V2499",
            "CTE-601-V2498",
            "CTE601S2115",
            "CTE605S2241",
            "CTE605S2496",
            "CTE-605-VXXXX",
            "CTE-605-V2111",
            "CTE-605-V2494",
            "CTE-605-V2470",
            "CTE-605-V2493",
            "CTE-605-V2497",
            "CTE-605-V2112",
            "CTE-605-2468",
            "CTE-605-2469",
            "CTE-605-2113",
            "CTE-605-V2115",
            "CTE-605-V2495",
            "CTE-605-V2467",
            "CTE-605-V2242",
            "CTE-605-V2471",
            "CTE-605-V2238",
            "CTE605S2240",
            "CTE-605-V2114",
            "CTE-605-V2239",
            "CTE605S2468",
            "CTE-605-V2468"
        };

        
        var deviceService = serviceProvider.GetRequiredService<TecCore.Services.Entra.DeviceService>();

        ICollection<Device> devices = new List<Device>();
        foreach (var device in deviceList)
        {
            Console.WriteLine($"Getting device: {device}");
            devices.Add(await deviceService.GetDeviceByDeviceNameAsync(device));
            Console.WriteLine($"Device {device} added.");
            
        }
        
        //export devices to csv
        var csv = new StringBuilder();
        csv.AppendLine("Id,DisplayName,Manufacturer,Model,OperatingSystem,OperatingSystemVersion");
        foreach (var device in devices)
        {
            csv.AppendLine($"{device.Id},{device.DisplayName},{device.Manufacturer},{device.Model},{device.OperatingSystem},{device.OperatingSystemVersion}");
        }
        await File.WriteAllTextAsync(@"D:\Gitec\Resources\Surfaces.csv", csv.ToString());
        
        
        


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