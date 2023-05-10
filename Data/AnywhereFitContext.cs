using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnywhereFit.Data
{
    public class AnywhereFitContext : IdentityDbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        private readonly IConfiguration? _config;

        public AnywhereFitContext(DbContextOptions<AnywhereFitContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public AnywhereFitContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }
    }
}
