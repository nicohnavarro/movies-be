using System.Net;
using KindaMovies.Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KindaMovies.Function
{
    public class GetMovies
    {
        private readonly ILogger<GetMovies> _logger;
        private readonly AppDbContext _dbContext;
        public GetMovies(ILogger<GetMovies> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("GetMovies")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function is called...");
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("SqlDb");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    SELECT 
                        Id,
                        Title,
                        TRY_CAST(ReleaseYear AS INT) as ReleaseYear,
                        Locations,
                        FunFacts,
                        ProductionCompany,
                        Distributor,
                        Director,
                        Writer,
                        Actor1,
                        Actor2,
                        Actor3,
                        Point,
                        Longitude,
                        Latitude,
                        AnalysisNeighborhood,
                        SupervisorDistrict,
                        DataAsOf,
                        DataLoadedAt,
                        TRY_CAST(SFFindNeighborhoods AS INT) as SFFindNeighborhoods,
                        TRY_CAST(AnalysisNeighborhoods AS INT) as AnalysisNeighborhoods,
                        TRY_CAST(CurrentSupervisorDistricts AS INT) as CurrentSupervisorDistricts
                    FROM Movies", connection);

                using var reader = await command.ExecuteReaderAsync();
                var movies = new List<dynamic>();

                while (await reader.ReadAsync())
                {
                    movies.Add(new
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? null : reader.GetValue(1).ToString(),
                        ReleaseYear = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                        Locations = reader.IsDBNull(3) ? null : reader.GetValue(3).ToString(),
                        FunFacts = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString(),
                        ProductionCompany = reader.IsDBNull(5) ? null : reader.GetValue(5).ToString(),
                        Distributor = reader.IsDBNull(6) ? null : reader.GetValue(6).ToString(),
                        Director = reader.IsDBNull(7) ? null : reader.GetValue(7).ToString(),
                        Writer = reader.IsDBNull(8) ? null : reader.GetValue(8).ToString(),
                        Actor1 = reader.IsDBNull(9) ? null : reader.GetValue(9).ToString(),
                        Actor2 = reader.IsDBNull(10) ? null : reader.GetValue(10).ToString(),
                        Actor3 = reader.IsDBNull(11) ? null : reader.GetValue(11).ToString(),
                        Point = reader.IsDBNull(12) ? null : reader.GetValue(12).ToString(),
                        Longitude = reader.IsDBNull(13) ? null : (double?)reader.GetDouble(13),
                        Latitude = reader.IsDBNull(14) ? null : (double?)reader.GetDouble(14),
                        AnalysisNeighborhood = reader.IsDBNull(15) ? null : reader.GetValue(15).ToString(),
                        SupervisorDistrict = reader.IsDBNull(16) ? null : reader.GetValue(16).ToString(),
                        DataAsOf = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DataLoadedAt = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        SFFindNeighborhoods = reader.IsDBNull(19) ? null : (int?)reader.GetInt32(19),
                        AnalysisNeighborhoods = reader.IsDBNull(20) ? null : (int?)reader.GetInt32(20),
                        CurrentSupervisorDistricts = reader.IsDBNull(21) ? null : (int?)reader.GetInt32(21)
                    });
                }

                return new OkObjectResult(movies);
            }
            catch (Exception ex)
            {    
                _logger.LogError(ex, "Error occurred while fetching movies");
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
}
