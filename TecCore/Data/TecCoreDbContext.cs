using Microsoft.EntityFrameworkCore;
using TecCore.Models;

namespace TecCore.Data
{
    public class TecCoreDbContext : DbContext
    {
        public TecCoreDbContext(DbContextOptions<TecCoreDbContext> options)
            : base(options)
        {
        }

        // DbSets for your models
        public DbSet<TecTask> TecTasks { get; set; }
        public DbSet<TecTaskUpdate> TecTaskUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BaseEntity properties if needed (applies to all inheriting entities)
            modelBuilder.Entity<TecTask>(entity =>
            {
                // Primary key coming from BaseEntity (Uid)
                entity.HasKey(t => t.Uid);

                // Enforce string length limits as per your model attributes
                entity.Property(t => t.TaskName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(t => t.TaskDescription)
                      .HasMaxLength(500);

                entity.Property(t => t.ContactInfo)
                      .HasMaxLength(100);

                // Set up a one-to-many relationship with TecTaskUpdate
                entity.HasMany(t => t.Updates)
                      .WithOne() // if you eventually add a navigation property in TecTaskUpdate, specify it here
                      .HasForeignKey("TecTaskUid")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TecTaskUpdate>(entity =>
            {
                entity.HasKey(tu => tu.Uid);

                entity.Property(tu => tu.Notes)
                      .HasMaxLength(1000);

            });
        }
    }
}
