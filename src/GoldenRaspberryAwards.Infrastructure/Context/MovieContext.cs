using GoldenRaspberryAwards.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Infrastructure.Context
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        }
    }
}
