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

        public async Task<List<DiameterModel>> GetDiametersAsync()
        {
            var diameters = new List<DiameterModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand("SELECT * FROM metric.get_diam('ASC')", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
               diameters.Add(new DiameterModel
                {
                    Id = 0,
                    Diameter = reader.GetDouble(0) 
                });
            }

            return diameters;
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
