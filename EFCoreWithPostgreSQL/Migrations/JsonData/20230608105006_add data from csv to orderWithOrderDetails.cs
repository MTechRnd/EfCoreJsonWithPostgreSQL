using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using System.Globalization;

#nullable disable

namespace EFCoreWithPostgreSQL.Migrations.JsonData
{
    /// <inheritdoc />
    public partial class adddatafromcsvtoorderWithOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = "./files/orderDetailWithJson.csv";
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CsvOrderDetailsWithJsonEntity>().ToList();
            using var dbContext = new JsonDataContext();

            var connectionString = "User ID=postgres;Password=1234;Server=localhost;Database=OrdersDB;Port=5433; IntegratedSecurity=true;Pooling=true;";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var tableName = "public.\"OrderWithOrderDetails\"";

            using var command = connection.CreateCommand();
            command.Transaction = transaction;

            var values = string.Join(", ", records.Select(r => $"('{r.CustomerName}', '{r.OrderDate}', '{r.OrderDetailsJson}')"));
            var sql = $"INSERT INTO {tableName} (\"CustomerName\", \"OrderDate\", \"OrderDetailsJson\") VALUES {values}";

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
