using Npgsql;
using ThreadApi.Features.MetricThread.Models;
using Serilog;

namespace ThreadApi.Features.MetricThread.Services
{
    public class MetricThreadService
    {
        private readonly string _connectionString;

        public MetricThreadService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres");
        }

        public async Task<List<DiameterModel>> GetMetricDiameters(string order)
        {
            var diameters = new List<DiameterModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

    // Ensure the order parameter is either ASC or DESC
    order = order.ToLowerInvariant() == "desc" ? "desc" : "asc";
    
    var query = $"SELECT DISTINCT diam, MIN(id) as id FROM metric.main GROUP BY diam ORDER BY diam {order}";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                diameters.Add(new DiameterModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    diam = "M " + reader.GetDouble(reader.GetOrdinal("diam")).ToString()
                });
            }

            return diameters;
        }
    }
}
