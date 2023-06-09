using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
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

            IConfiguration config = new ConfigurationBuilder()
               .AddUserSecrets<DataContext>()
               .Build();
            using var connection = new NpgsqlConnection(config.GetConnectionString("LocalhostConnection"));
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
