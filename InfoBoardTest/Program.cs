﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TecCore.Data;
using TecCore.Models;
using TecCore.Models.InfoBoard;
using TecCore.Services;
using TecCore.Utilities;

class Program
{
    public static async Task Main(string[] args)
    {
        // Set up Dependency Injection
        var serviceProvider = new ServiceCollection()
            .AddSingleton<ConfigService>() // Assuming ConfigService is needed for database path
            .AddDbContext<InfoBoardDbContext>(options =>
                options.UseSqlite($"Data Source={ConfigService.DatabaseFile}"))
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InfoBoardDbContext>();
            
        Console.WriteLine("🏁 Running Database Tests...");

        // Ensure database is created
        await dbContext.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ Database Initialized.");

        // Test Admin User Creation
        var admin = dbContext.CreateAdminUser("admin", "password123");
        Console.WriteLine($"✅ Created Admin User: {admin.Username}");

        // Test Authentication
        try
        {
            dbContext.AuthenticateAdminUser("admin", "wrongpassword");
            Console.WriteLine("❌ Authentication should have failed!");
        }
        catch
        {
            Console.WriteLine("✅ Authentication failed with wrong password (expected).");
        }

        try
        {
            dbContext.AuthenticateAdminUser("admin", "password123");
            Console.WriteLine("✅ Authentication succeeded with correct password.");
        }
        catch
        {
            Console.WriteLine("❌ Authentication failed unexpectedly!");
        }

        // Test InfoBoard Creation
        var infoBoard = dbContext.CreateInfoBoard();
        Console.WriteLine($"✅ Created InfoBoard: {infoBoard.Uid}"); 

        // Test InfoBoardItem Creation
        var item = new InfoBoardItemText { Content = "Hello, World!" };
        dbContext.CreateInfoBoardItem(item);
        Console.WriteLine($"✅ Created InfoBoardItem: {item.Uid}, Content: {item.Content}");

        // Test InfoBoardItem Update
        item.Content = "Updated Content";
        dbContext.UpdateInfoBoardItem(item);
        Console.WriteLine($"✅ Updated InfoBoardItem: {item.Uid}, New Content: {item.Content}");

        // Check Automatic Timestamp Update
        var updatedItem = dbContext.GetInfoBoardItem(item.Uid);
        Console.WriteLine($"✅ UpdatedAt Check: {updatedItem.UpdatedAt}");

        // Test Soft Delete
        dbContext.DeleteInfoBoardItem(item);
        var deletedItem = dbContext.GetInfoBoardItem(item.Uid);
        Console.WriteLine($"✅ Soft Deleted Item? {deletedItem.IsDeleted}");

        Console.WriteLine("🏁 All Tests Completed!");
    }
}
