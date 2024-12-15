using GoldenRaspberryAwards.Domain.Interfaces;
using GoldenRaspberryAwards.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoldenRaspberryAwards.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv([FromForm] FileUploadDto file)
        {
            try
            {
                var count = await _movieService.ProcessCsvFileAsync(file.File);
                return Ok(new { Message = "Arquivo processado com sucesso.", MoviesCount = count });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar o arquivo: {ex.Message}");
            }
        }

        [HttpGet("producers/intervals")]
        public async Task<IActionResult> GetProducersAwardIntervals()
        {
            var result = await _movieService.GetProducersAwardIntervalsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> AllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

    }
}
