using System.Net;
using KindaMovies.Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

                var students = await _dbContext.Movies.ToListAsync();


                return new OkObjectResult(students);

            }
            catch (Exception ex)
            {    
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
