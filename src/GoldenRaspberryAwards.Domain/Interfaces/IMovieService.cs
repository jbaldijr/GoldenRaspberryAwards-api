using GoldenRaspberryAwards.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace GoldenRaspberryAwards.Domain.Interfaces
{
    public interface IMovieService
    {
        Task<int> ProcessCsvFileAsync(IFormFile file);
        Task<object> GetProducersAwardIntervalsAsync();
        Task<List<Movie>> GetAllMoviesAsync();
    }
}
