using GoldenRaspberryAwards.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace GoldenRaspberryAwards.Domain.Interfaces
{
    public interface IMovieService
    {
        Task<int> ProcessCsvFileAsync(IFormFile file);
        Task<IntervalResult> GetProducersAwardIntervalsAsync();
        Task<List<Movie>> GetAllMoviesAsync();
    }
}
