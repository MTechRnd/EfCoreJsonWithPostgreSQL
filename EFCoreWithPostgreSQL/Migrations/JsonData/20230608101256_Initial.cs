using System;
using System.Collections.Generic;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreWithPostgreSQL.Migrations.JsonData
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderWithOrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CustomerName = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "date", nullable: false),
                    OrderDetailsJson = table.Column<IList<OrderDetailsJson>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderWithOrderDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderWithOrderDetails");
        }
    }
}
