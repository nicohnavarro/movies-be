using System.Net;
using KindaMovies.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KindaMovies.Function
{
    public class GetPaginatedMovies
    {
        private readonly ILogger<GetPaginatedMovies> _logger;
        private readonly IMovieService _movieService;

        public GetPaginatedMovies(ILogger<GetPaginatedMovies> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [Function("GetPaginatedMovies")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/movie/pag")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function GetPaginatedMovies is called...");
            try
            {
                // Get query parameters
                var pageNumberStr = req.Query["pageNumber"].ToString();
                var pageSizeStr = req.Query["pageSize"].ToString();

                var pageNumber = string.IsNullOrEmpty(pageNumberStr) ? 1 : int.Parse(pageNumberStr);
                var pageSize = string.IsNullOrEmpty(pageSizeStr) ? 10 : int.Parse(pageSizeStr);

                var (movies, totalCount) = await _movieService.GetPaginatedMoviesAsync(pageNumber, pageSize);
                
                return new OkObjectResult(new { 
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Movies = movies 
                });
            }
            catch (Exception ex)
            {    
                _logger.LogError(ex, "Error occurred while fetching paginated movies");
                return new BadRequestObjectResult($"Error: {ex.Message}");
            }
        }
    }
} 