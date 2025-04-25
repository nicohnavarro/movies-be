using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using KindaMovies.Domain.Models;
using KindaMovies.Domain.Interfaces;
using KindaMovies.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KindaMovies.Infraestructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Movies.CountAsync();
            var movies = await _context.Movies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<IEnumerable<Movie>> SearchByTermAsync(string searchTerm)
        {
            return await _context.Movies
                .Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();
        }
    }
} 