using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TecCore.Data;

public class TecCoreDbContextFactory : IDesignTimeDbContextFactory<TecCoreDbContext>
{
    public TecCoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TecCoreDbContext>();
        optionsBuilder.UseSqlite($"Data Source={ConfigService.DatabaseFile}"); // Change to match your database provider

        return new TecCoreDbContext(optionsBuilder.Options);
    }
}