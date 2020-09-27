using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using MyWebApp.Class;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;

namespace MyWebApp.Models
{
    public class ConfigContext : DbContext
    {
        private readonly IDBContextConfiguration _configuration;
        public DbSet<Project> Porjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HealthData> HealthDatas { get; set; }


        public ConfigContext(DbContextOptions<ConfigContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("Server=127.0.0.1;port=3306;Database=configdb;uid=root;pwd=yang123");
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Port=5432;Password=postgres");
            //optionsBuilder.UseNpgsql("Host=172.17.0.2;Username=postgres;Port=5432;Password=postgres");
            //optionsBuilder.UseSqlite("Data Source=Config.db");
            optionsBuilder.EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Project>().HasKey(m => m.ID);
            builder.Entity<User>().HasKey(m => m.ID);
            builder.Entity<HealthData>().HasKey(m => m.ProjectID);
            base.OnModelCreating(builder);
            
        }
    }
}
