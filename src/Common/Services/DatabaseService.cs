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

        public async Task<List<DiameterModel>> GetMetricDiameters(string order)
        {
            var diameters = new List<DiameterModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Ensure the order parameter is either ASC or DESC
            order = order.ToLowerInvariant() == "desc" ? "desc" : "asc";
            
            var query = $"SELECT id, diam FROM metric.main ORDER BY diam {order}";

            using var cmd = new NpgsqlCommand(query, conn);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                diameters.Add(new DiameterModel
                {
                    // Assuming there's an 'id' column in the 'metric.main' table
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    // Use GetDouble for double precision data type
                    diam = "M " + reader.GetDouble(reader.GetOrdinal("diam")).ToString()
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
