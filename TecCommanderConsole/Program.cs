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
        
        var graphService = serviceProvider.GetRequiredService<MsGraphService>();
        //
        // var users = graphService.GetUsersAsync().Result;
        // var groups = graphService.GetGroupsAsync().Result;
        // var devices = graphService.GetDevicesAsync().Result;
        // var AllUsers = graphService.GetAllUsersAsync().Result;
        
        var staff = graphService.GetAllStaffAsync().Result;
        var arthur = graphService.GetUsersByLastNameAsync("Cullen").Result;
        var curr = graphService.GetAllCurrentStudentsAsync().Result;
        
        // Arthurs
        Console.WriteLine("========================  'ARTHURS'  ========================");
        Console.WriteLine();
        Console.WriteLine($"{"Display Name",-25} {"Given Name",-15} {"Surname",-15} {"Job Title",-30} {"Department",-20} {"Employee Type",-15}");
        Console.WriteLine(new string('-', 110)); // Horizontal separator
        
        foreach (var user in arthur)
        {
            Console.WriteLine($"{user.DisplayName,-25} {user.GivenName,-15} {user.Surname,-15} {user.JobTitle,-30} {user.Department,-20} {user.EmployeeType,-15}");
        }
        
        Console.WriteLine(); // Add some space at the end


        // STAFF
        Console.WriteLine("========================  'STAFF'  ========================");
        Console.WriteLine();
        Console.WriteLine($"{"Display Name",-25} {"Given Name",-15} {"Surname",-15} {"Job Title",-30} {"Department",-20}");
        Console.WriteLine(new string('-', 110)); // Horizontal separator

        foreach (var staffmember in staff)
        {
            Console.WriteLine($"{staffmember.DisplayName,-25} {staffmember.GivenName,-15} {staffmember.Surname,-15} {staffmember.JobTitle,-30} {staffmember.Department,-20}");
        }

        Console.WriteLine(); // Add some space at the end
        var students = graphService.GetAllStudentsAsync().Result;

        // STUDENTS
        Console.WriteLine("======================== 'STUDENTS' ========================");
        Console.WriteLine($"{"Display Name",-25} {"Given Name",-15} {"Surname",-15} {"Job Title",-30} {"Department",-20}");
        Console.WriteLine(new string('-', 110)); // Horizontal separator

        foreach (var student in students)
        {
            Console.WriteLine($"{student.DisplayName,-25} {student.GivenName,-15} {student.Surname,-15} {student.JobTitle,-30} {student.Department,-20}");
        }

        Console.WriteLine(); // Add some space at the end
        

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