using GoldenRaspberryAwards.Api.Configurations;
using GoldenRaspberryAwards.Application.Services;
using GoldenRaspberryAwards.Domain.Interfaces;
using GoldenRaspberryAwards.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona serviços ao container
        ConfigureServices(builder);

        var app = builder.Build();

        // Configura o pipeline HTTP
        ConfigurePipeline(app);

        app.Run();
    }

    // Expor o método para WebApplicationFactory nos testes
    public static WebApplicationBuilder CreateWebApplicationBuilder(string[] args) =>
        WebApplication.CreateBuilder(args);

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddDbContext<MovieContext>(options =>
            options.UseInMemoryDatabase("GoldenRaspberryAwards"));
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerConfiguration.ConfigureSwaggerGen);
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Configura o Swagger apenas no ambiente de desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(SwaggerConfiguration.ConfigureSwaggerUI);
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
