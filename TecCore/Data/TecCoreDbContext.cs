using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using TecCore.Models;
using TecCore.Models.Course;
using TecCore.Models.Location;
using TecCore.Models.People;

namespace TecCore.Data
{
    public class TecCoreDbContext : DbContext
    {
        
        public TecCoreDbContext(DbContextOptions<TecCoreDbContext> options) : base(options) { }

        //Locations
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Office> Offices { get; set; }
        
        //People
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }
        public DbSet<ParaPro> ParaPros { get; set; }
        
        //Courses
        public DbSet<EduProgram> EduPrograms { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Classroom>().HasKey(c => c.Uid);
            modelBuilder.Entity<Office>().HasKey(o => o.Uid);
            modelBuilder.Entity<Lab>().HasKey(o => o.Uid);
            
            modelBuilder.Entity<Instructor>().HasKey(i => i.Uid);
            modelBuilder.Entity<Staff>().HasKey(s => s.Uid);
            modelBuilder.Entity<ParaPro>().HasKey(u => u.Uid);
            
            modelBuilder.Entity<EduProgram>().HasKey(e => e.Uid);
            
            // Configure unique constraints or relationships if needed
            
            modelBuilder.Entity<EduProgram>()
                .HasOne(p => p.Classroom)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<EduProgram>()
                .HasOne(p => p.Instructor)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<EduProgram>()
                .HasMany(p => p.Labs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<EduProgram>()
                .HasMany(p => p.ParaPros)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Office>()
                .HasMany(p => p.Staffs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
                
                
                
                
        }
    }
}