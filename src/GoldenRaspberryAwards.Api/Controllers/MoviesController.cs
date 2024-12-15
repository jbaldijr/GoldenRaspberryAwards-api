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

        /// <summary>
        /// Realiza o upload de um arquivo CSV contendo informações de filmes.
        /// </summary>
        /// <remarks>
        /// O arquivo deve conter os seguintes campos: `year`, `title`, `studios`, `producers`, `winner`.
        /// </remarks>
        /// <param name="file">Arquivo CSV a ser processado.</param>
        /// <returns>Mensagem de sucesso e o número de filmes processados.</returns>
        /// <response code="200">Arquivo processado com sucesso.</response>
        /// <response code="400">Arquivo inválido.</response>
        /// <response code="500">Erro interno ao processar o arquivo.</response>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
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

        /// <summary>
        /// Obtém os produtores com os menores e maiores intervalos entre vitórias.
        /// </summary>
        /// <returns>Lista com os intervalos mínimos e máximos.</returns>
        /// <response code="200">Lista de intervalos retornada com sucesso.</response>
        [HttpGet("producers/intervals")]
        [ProducesResponseType(typeof(IEnumerable<Movie>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Movie>), 500)]
        public async Task<IActionResult> GetProducersAwardIntervals()
        {
            var result = await _movieService.GetProducersAwardIntervalsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retorna todos os filmes processados.
        /// </summary>
        /// <returns>Lista de filmes.</returns>
        /// <response code="200">Lista de filmes retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Movie>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Movie>), 500)]
        public async Task<IActionResult> AllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

    }
}
