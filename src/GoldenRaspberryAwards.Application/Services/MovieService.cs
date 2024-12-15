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

        public async Task<IntervalResult> GetProducersAwardIntervalsAsync()
        {
            var producers = _context.Movies
                .Where(m => m.IsWinner)
                .AsEnumerable()
                .SelectMany(m => m.Producers
                    .Split(new[] { ",", " and " }, StringSplitOptions.TrimEntries)
                    .Select(producer => new { Producer = producer, Year = m.Year }))
                .GroupBy(p => p.Producer)
                .Select(g => new
                {
                    Producer = g.Key,
                    Wins = g.Select(p => p.Year).OrderBy(year => year).ToList()
                })
                .Where(p => p.Wins.Count > 1)
                .ToList();

            var intervals = producers.SelectMany(p => p.Wins.Zip(p.Wins.Skip(1), (prev, next) => new ProducerInterval
            {
                Producer = p.Producer,
                Interval = next - prev,
                PreviousWin = prev,
                FollowingWin = next
            }));

            var minInterval = intervals.GroupBy(i => i.Interval).OrderBy(g => g.Key).FirstOrDefault();
            var maxInterval = intervals.GroupBy(i => i.Interval).OrderByDescending(g => g.Key).FirstOrDefault();

            return new IntervalResult
            {
                Min = minInterval?.ToList() ?? new List<ProducerInterval>(),
                Max = maxInterval?.ToList() ?? new List<ProducerInterval>()
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
