using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Seeding;

namespace Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleEntity>().HasData(SimpleEntitySeedData.SimpleEntityData());
        }
    }
}
