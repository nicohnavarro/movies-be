using System.Net;
using KindaMovies.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KindaMovies.Function
{
    public class SearchMovies
    {
        private readonly ILogger<SearchMovies> _logger;
        private readonly IMovieService _movieService;

        public SearchMovies(ILogger<SearchMovies> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [Function("SearchMovies")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/movie/search")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function SearchMovies is called...");
            try
            {
                var searchTerm = req.Query["term"].ToString();
                
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new BadRequestObjectResult("Search term parameter is required");
                }

                var movies = await _movieService.SearchMoviesByTermAsync(searchTerm);
                return new OkObjectResult(new { 
                    SearchTerm = searchTerm,
                    Movies = movies 
                });
            }
            catch (Exception ex)
            {    
                _logger.LogError(ex, "Error occurred while searching movies");
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
} 