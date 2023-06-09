using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Formats.Asn1;
using System.Globalization;

#nullable disable

namespace EFCoreWithPostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class adddatafromcsvtoorders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = "./files/orders.csv";
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CsvOrderEntity>().ToList();
            IConfiguration config = new ConfigurationBuilder()
               .AddUserSecrets<DataContext>()
               .Build();
            using var connection = new NpgsqlConnection(config.GetConnectionString("LocalhostConnection"));
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var tableName = "public.\"Orders\"";

            using var command = connection.CreateCommand();
            command.Transaction = transaction;

            var values = string.Join(", ", records.Select(r => $"('{r.Id}','{r.CustomerName}', '{r.OrderDate}')"));
            var sql = $"INSERT INTO {tableName} (\"Id\",\"CustomerName\", \"OrderDate\") VALUES {values}";

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
