using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "netCore3Test");

            migrationBuilder.CreateTable(
                name: "simpleEntities",
                schema: "netCore3Test",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdateBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_simpleEntities", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "netCore3Test",
                table: "simpleEntities",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdatedDate", "Name" },
                values: new object[] { new Guid("bb3d93e8-399d-4a0a-93bb-765dbe5c44d8"), "Data Seeder", new DateTime(2019, 12, 6, 21, 38, 17, 3, DateTimeKind.Utc).AddTicks(717), "DataSeeder", new DateTime(2019, 12, 6, 21, 38, 17, 3, DateTimeKind.Utc).AddTicks(1821), "TestName 1" });

            migrationBuilder.InsertData(
                schema: "netCore3Test",
                table: "simpleEntities",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "LastUpdateBy", "LastUpdatedDate", "Name" },
                values: new object[] { new Guid("0384812c-c8f5-45f7-819a-13d737b926be"), "Data Seeder", new DateTime(2019, 12, 6, 21, 38, 17, 3, DateTimeKind.Utc).AddTicks(2908), "DataSeeder", new DateTime(2019, 12, 6, 21, 38, 17, 3, DateTimeKind.Utc).AddTicks(2926), "TestName 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "simpleEntities",
                schema: "netCore3Test");
        }
    }
}
