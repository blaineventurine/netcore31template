using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Extensions;

namespace Persistence
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SimpleEntity> SimpleEntities { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SimpleEntityConfiguration());

            modelBuilder.Seed();
        }
    }
}
