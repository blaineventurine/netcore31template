using System;
using System.Collections.Generic;
using Domain.Models;

namespace Persistence.Seeding
{
    public static class SimpleEntitySeedData
    {
        public static IEnumerable<SimpleEntity> SimpleEntityData()
        {
            var seedData = new List<SimpleEntity>
            {
                new SimpleEntity(Guid.NewGuid(), "TestName 1"),
                new SimpleEntity(Guid.NewGuid(), "TestName 2")
            };

            seedData.ForEach(x =>
            {
                x.CreatedDate = DateTime.UtcNow;
                x.CreatedBy = "Data Seeder";
                x.LastUpdatedDate = DateTime.UtcNow;
                x.LastUpdateBy = "DataSeeder";
            });

            return seedData;
        }
    }
}
