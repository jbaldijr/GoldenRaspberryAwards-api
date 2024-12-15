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
                new Movie { Year = 2008, Title = "Movie A", Studios = "Studio A", Producers = "Producer 1", IsWinner = true },
                new Movie { Year = 2009, Title = "Movie B", Studios = "Studio B", Producers = "Producer 1", IsWinner = true },
                new Movie { Year = 1900, Title = "Movie C", Studios = "Studio C", Producers = "Producer 1", IsWinner = true },
                new Movie { Year = 1999, Title = "Movie D", Studios = "Studio D", Producers = "Producer 1", IsWinner = true }
            });
            context.SaveChanges();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
