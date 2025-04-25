using System.Collections.Generic;
using System.Threading.Tasks;
using KindaMovies.Domain.Models;

namespace KindaMovies.Domain.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPaginatedMoviesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Movie>> SearchMoviesByTermAsync(string searchTerm);
    }
} 