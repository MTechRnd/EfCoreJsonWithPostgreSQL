using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using System.Globalization;

#nullable disable

namespace EFCoreWithPostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class adddatafromcsvtoorderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = "./files/orderDetails.csv";
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CsvOrderDetailsEntity>().ToList();
            using var dbContext = new DataContext();

            var connectionString = "User ID=postgres;Password=1234;Server=localhost;Database=OrdersDB;Port=5433; IntegratedSecurity=true;Pooling=true;";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var tableName = "public.\"OrderDetails\"";

            using var command = connection.CreateCommand();
            command.Transaction = transaction;

            var values = string.Join(", ", records.Select(r => $"('{r.OrderId}','{r.ItemName}', '{r.Price}','{r.Quantity}')"));
            var sql = $"INSERT INTO {tableName} (\"OrderId\",\"ItemName\",\"Price\",\"Quantity\") VALUES {values}";

            command.CommandText = sql;
            command.ExecuteNonQuery();

            transaction.Commit();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
