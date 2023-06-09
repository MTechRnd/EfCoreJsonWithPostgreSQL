using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
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
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<DataContext>()
                .Build();
            using var connection = new NpgsqlConnection(config.GetConnectionString("LocalhostConnection"));
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
