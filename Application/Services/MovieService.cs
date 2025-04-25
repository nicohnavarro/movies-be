using System.Collections.Generic;
using System.Threading.Tasks;
using KindaMovies.Domain.Interfaces;
using KindaMovies.Domain.Models;

namespace KindaMovies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _movieRepository.GetAllAsync();
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPaginatedMoviesAsync(int pageNumber, int pageSize)
        {
            return await _movieRepository.GetPaginatedAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Movie>> SearchMoviesByTermAsync(string searchTerm)
        {
            var (movies, _) = await _movieRepository.GetPaginatedAsync(1, int.MaxValue);
            return movies.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()));
        }
    }
} 