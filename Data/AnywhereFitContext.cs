using Microsoft.EntityFrameworkCore;

namespace AnywhereFit.Data
{
    public class AnywhereFitContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=workouts.db");
        }
    }
}
