using CsvHelper;
using CsvHelper.Configuration;
using GoldenRaspberryAwards.Domain.Interfaces;
using GoldenRaspberryAwards.Domain.Models;
using GoldenRaspberryAwards.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GoldenRaspberryAwards.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieContext _context;

        public MovieService(MovieContext context)
        {
            _context = context;
        }

        public async Task<int> ProcessCsvFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("O arquivo CSV é obrigatório.");

            using var stream = file.OpenReadStream();
            var movies = ImportMoviesFromStream(stream);

            // Limpar banco antes de inserir novos registros
            _context.Movies.RemoveRange(_context.Movies);
            await _context.SaveChangesAsync();

            // Adicionar novos registros
            await _context.Movies.AddRangeAsync(movies);
            await _context.SaveChangesAsync();

            return movies.Count;
        }

        public async Task<object> GetProducersAwardIntervalsAsync()
        {
            var producers = _context.Movies
                .Where(m => m.IsWinner)
                .GroupBy(m => m.Producers)
                .Select(g => new
                {
                    Producer = g.Key,
                    Awards = g.OrderBy(m => m.Year).Select(m => m.Year).ToList()
                })
                .ToList();

            var intervals = producers
                .Where(p => p.Awards.Count > 1) // Verifica se há mais de um prêmio
                .Select(p => new
                {
                    Producer = p.Producer,
                    MinInterval = p.Awards.Skip(1).Zip(p.Awards, (a, b) => a - b).Min(),
                    MaxInterval = p.Awards.Skip(1).Zip(p.Awards, (a, b) => a - b).Max()
                });

            var min = intervals.OrderBy(i => i.MinInterval).FirstOrDefault();
            var max = intervals.OrderByDescending(i => i.MaxInterval).FirstOrDefault();

            return new
            {
                Min = min,
                Max = max
            };
        }


        private static List<Movie> ImportMoviesFromStream(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null, // Ignore missing headers
                MissingFieldFound = null // Ignore missing fields
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<MovieMap>(); // Registra o mapeamento
            return csv.GetRecords<Movie>().ToList();
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }
    }
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Year).Name("year");
            Map(m => m.Title).Name("title");
            Map(m => m.Studios).Name("studios");
            Map(m => m.Producers).Name("producers");
            Map(m => m.IsWinner).Name("winner").Convert(args =>
            {
                var field = args.Row.TryGetField("winner", out string value) ? value : null;
                return value?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false;
            });
        }
    }
}
