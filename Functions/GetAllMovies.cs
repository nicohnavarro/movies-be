using System.Net;
using KindaMovies.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KindaMovies.Function
{
    public class GetAllMovies
    {
        private readonly ILogger<GetAllMovies> _logger;
        private readonly IMovieService _movieService;

        public GetAllMovies(ILogger<GetAllMovies> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [Function("GetAllMovies")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/movie/all")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function GetAllMovies is called...");
            try
            {
                var movies = await _movieService.GetAllMoviesAsync();
                return new OkObjectResult(new { Movies = movies });
            }
            catch (Exception ex)
            {    
                _logger.LogError(ex, "Error occurred while fetching all movies");
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
} 