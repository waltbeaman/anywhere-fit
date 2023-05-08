using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AnywhereFit.Data
{
    public class WorkoutLogStorage : IWorkoutLogStorage
    {
        private readonly AnywhereFitContext _dbContext;

        public WorkoutLogStorage(AnywhereFitContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddWorkoutAsync(List<Exercise> workout, string userId)
        {
            var now = DateTime.Now;

            foreach (var exercise in workout)
            {
                exercise.UserId = userId;
                exercise.DateTime = now;
                await _dbContext.Exercises.AddAsync(exercise);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Exercise>> GetAllUserExercisesAsync(string userId)
        {
            var exercises = await _dbContext.Exercises.Where(e => e.UserId == userId).ToListAsync(); ;

            return exercises;
        }
    }
}