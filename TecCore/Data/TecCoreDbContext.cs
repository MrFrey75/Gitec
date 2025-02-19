using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using TecCore.Models;

namespace TecCore.Data
{
    public class TecCoreDbContext : DbContext
    {
        
        public TecCoreDbContext(DbContextOptions<TecCoreDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<gStudent> gStudents { get; set; }
        public DbSet<eStudent> eStudents { get; set; }
        
        public DbSet<Staff> Staff { get; set; }
        public DbSet<gStaff> gStaff { get; set; }
        public DbSet<eStaff> eStaff { get; set; }
        
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<gFaculty> gFaculty { get; set; }
        public DbSet<eFaculty> eFaculty { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasKey(c => c.Uid);
            modelBuilder.Entity<gStudent>().HasKey(c => c.Uid);
            modelBuilder.Entity<eStudent>().HasKey(c => c.Uid);

            modelBuilder.Entity<Staff>().HasKey(c => c.Uid);
            modelBuilder.Entity<gStaff>().HasKey(c => c.Uid);
            modelBuilder.Entity<eStaff>().HasKey(c => c.Uid);

            modelBuilder.Entity<Faculty>().HasKey(c => c.Uid);
            modelBuilder.Entity<gFaculty>().HasKey(c => c.Uid);
            modelBuilder.Entity<eFaculty>().HasKey(c => c.Uid);
                
        }
    }
}