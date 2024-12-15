using GoldenRaspberryAwards.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Infrastructure.Context
{
    public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
    {
        public required DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        }
    }
}
