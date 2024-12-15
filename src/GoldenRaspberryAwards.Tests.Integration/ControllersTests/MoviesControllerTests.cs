using FluentAssertions;
using GoldenRaspberryAwards.Domain.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace GoldenRaspberryAwards.Tests.Integration.ControllersTests
{
    public class MoviesControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task UploadCsv_ShouldProcessCsvFile()
        {
            // Arrange
            var fileContent = new ByteArrayContent(LoadEmbeddedResource("GoldenRaspberryAwards.Tests.Integration.TestFiles.movies.csv"));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            using var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "file", "movies.csv");

            // Act
            var response = await _client.PostAsync("/api/movies/upload", formData);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("Arquivo processado com sucesso");
        }

        [Fact]
        public async Task GetProducersAwardIntervals_ShouldReturnIntervals()
        {
            // Act
            var response = await _client.GetAsync("/api/movies/producers/intervals");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("min").And.Contain("max");
        }

        [Fact]
        public async Task GetProducersIntervals_ShouldReturnCorrectIntervals()
        {

            // Act: Chamar o endpoint que retorna os intervalos
            var response = await _client.GetAsync("/api/movies/producers/intervals");

            // Assert: Verificar o resultado
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IntervalResult>();
            result.Should().NotBeNull();

            // Validar os intervalos mínimos e máximos
            result?.Min.Should().ContainSingle(p => p.Producer == "Producer 1" && p.Interval == 1);
            result?.Max.Should().ContainSingle(p => p.Producer == "Producer 1" && p.Interval == 99);
        }

        private byte[] LoadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Resource not found: {resourceName}");
            }

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
