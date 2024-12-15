using GoldenRaspberryAwards.Api.Configurations;
using GoldenRaspberryAwards.Application.Services;
using GoldenRaspberryAwards.Domain.Interfaces;
using GoldenRaspberryAwards.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseInMemoryDatabase("GoldenRaspberryAwards"));
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.ConfigureSwaggerGen);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(SwaggerConfiguration.ConfigureSwaggerUI);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
