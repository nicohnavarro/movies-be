using System.Collections.Generic;
using System.Threading.Tasks;
using KindaMovies.Domain.Models;

namespace KindaMovies.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Movie>> SearchByTermAsync(string searchTerm);
    }
} 