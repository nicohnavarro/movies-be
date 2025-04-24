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
                var count = await _dbContext.Movies.CountAsync();
                var movies = await _dbContext.Movies.ToListAsync();

                return new OkObjectResult(new { 
                    TotalRecords = count,
                    Movies = movies 
                });
            }
            catch (Exception ex)
            {    
                _logger.LogError(ex, "Error occurred while fetching movies");
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
}
