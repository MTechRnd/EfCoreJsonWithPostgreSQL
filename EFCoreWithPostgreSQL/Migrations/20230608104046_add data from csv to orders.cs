using CsvHelper;
using EFCoreJsonApp.Data;
using EFCoreJsonApp.Models.CsvDataReadModels;
using Microsoft.EntityFrameworkCore.Migrations;
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
            using var dbContext = new DataContext();
            var connectionString = $"User ID=postgres;Password=1234;Server=localhost;Database=OrdersDB;Port=5433; IntegratedSecurity=true;Pooling=true;";
            using var connection = new NpgsqlConnection(connectionString);
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
