using GoldenRaspberryAwards.Domain.Models;
using GoldenRaspberryAwards.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GoldenRaspberryAwards.Tests.Integration
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTestBase()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove a configuração padrão do banco de dados
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<MovieContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);

                        // Configura o banco de dados InMemory
                        services.AddDbContext<MovieContext>(options =>
                            options.UseInMemoryDatabase("TestDatabase"));

                        // Seed de dados no banco de dados InMemory
                        var sp = services.BuildServiceProvider();
                        using var scope = sp.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
                        db.Database.EnsureCreated();
                        SeedTestData(db);
                    });
                });

            _client = _factory.CreateClient();
        }

        private void SeedTestData(MovieContext context)
        {
            context.Movies.AddRange(new[]
            {
                new Movie { Year = 2000, Title = "Movie A1", Studios = "Studio A", Producers = "Producer A", IsWinner = true },
                new Movie { Year = 2002, Title = "Movie A2", Studios = "Studio A", Producers = "Producer A", IsWinner = true },

                new Movie { Year = 2001, Title = "Movie B1", Studios = "Studio B", Producers = "Producer B", IsWinner = true },
                new Movie { Year = 2003, Title = "Movie B2", Studios = "Studio B", Producers = "Producer B", IsWinner = true },

                // Dados para intervalo máximo (100 anos de diferença)
                new Movie { Year = 1900, Title = "Movie C1", Studios = "Studio C", Producers = "Producer C", IsWinner = true },
                new Movie { Year = 2000, Title = "Movie C2", Studios = "Studio C", Producers = "Producer C", IsWinner = true },

                new Movie { Year = 1905, Title = "Movie D1", Studios = "Studio D", Producers = "Producer D", IsWinner = true },
                new Movie { Year = 2005, Title = "Movie D2", Studios = "Studio D", Producers = "Producer D", IsWinner = true }
            });
            context.SaveChanges();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
