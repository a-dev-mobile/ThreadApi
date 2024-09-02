using Npgsql;
using System.Data;
using Serilog;
using ThreadApi.Features.MetricThread.Models;

namespace ThreadApi.Common.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres");
            CheckDatabaseConnection().Wait();
        }



        private async Task CheckDatabaseConnection()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();
                Log.Information("Database connection successful.");
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to connect to the database.");
            }
        }
    }
}
