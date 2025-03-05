using Microsoft.EntityFrameworkCore;
using TecCore.Models;
using TecCore.Services;

namespace TecCore.Data
{
    public class TecCoreDbContext : DbContext
    {
        public TecCoreDbContext(DbContextOptions<TecCoreDbContext> options)
            : base(options)
        {
        }

        public TecCoreDbContext()
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={ConfigService.DatabaseFile}"); // Ensure SQLite is configured
            }
        }

        // DbSets for your models
        public DbSet<GitecUser> Users { get; set; }
        public DbSet<GitecDevice> Devices { get; set; }
        public DbSet<RoomLocation> Rooms { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<GitecUser>(entity =>
            {
                entity.HasKey(u => u.Uid);
            });
            
            modelBuilder.Entity<GitecDevice>(entity =>
            {
                entity.HasKey(d => d.Uid);
            });
            
            modelBuilder.Entity<RoomLocation>(entity =>
            {
                entity.HasKey(r => r.Uid);
            });
        }
    }
}
