using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace AnywhereFit.Data
{
    public class WorkoutLogStorage : IWorkoutLogStorage
    {
        private readonly AnywhereFitContext _dbContext;
        private readonly ExerciseService _exerciseService;

        public WorkoutLogStorage(AnywhereFitContext dbContext, ExerciseService exerciseService)
        {
            _dbContext = dbContext;
            _exerciseService = exerciseService;
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

        public async Task<List<WeeklyReps>> GetWeeklyRepsAsync(string userId)
        {
            List<string> upperBody = new List<string> { "back", "chest", "lower arms", "upper arms", "neck", "shoulders" };
            List<string> lowerBody = new List<string> { "lower legs", "upper legs", "waist" };

            var startDate = DateTime.Today.AddDays(-90);

            var weeklyReps = await _dbContext.Exercises
                .Where(e => e.UserId == userId && e.DateTime.HasValue && e.DateTime.Value >= startDate)
                .GroupBy(e => new { Year = e.DateTime.Value.Year, Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.DateTime.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) })
                .Select(g => new WeeklyReps
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    StartDate = g.Min(e => e.DateTime.Value),
                    TotalReps = (int)g.Sum(e => e.Reps),
                    UpperBodyReps = (int)g.Where(e => upperBody.Contains(e.BodyPart)).Sum(e => e.Reps),
                    LowerBodyReps = (int)g.Where(e => lowerBody.Contains(e.BodyPart)).Sum(e => e.Reps),
                    CardioReps = (int)g.Where(e => e.BodyPart == "cardio").Sum(e => e.Reps)
                }).ToListAsync();

            return weeklyReps;
        }


        // The following methods are for DEV ONLY. Do not move to production.
        public async Task AddDummyDataAsync(string userId)
        {

            var random = new Random();
            var dummyData = new List<Exercise>();
            var exercises = await _exerciseService.GetExercises();

            for (int i  = 0; i < 200; i++)
            {
                DateTime randomDate = DateTime.Now.AddDays(-random.Next(1, 90));
                int randomReps = random.Next(6, 61);
                var randomExercise = exercises[random.Next(exercises.Count)];

                var exercise = new Exercise
                {
                    Name = randomExercise.Name + "-DUMMY",
                    BodyPart = randomExercise.BodyPart,
                    TargetMuscle = randomExercise.TargetMuscle,
                    Equipment = randomExercise.Equipment,
                    Reps = randomReps,
                    DateTime = randomDate,
                    UserId = userId
                };
                dummyData.Add(exercise);
            }

            await _dbContext.Exercises.AddRangeAsync(dummyData);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDummyDataAsync(string userId)
        {
            var dummyExercises = await _dbContext.Exercises
                .Where(e => e.UserId == userId && e.Name.Contains("DUMMY")).ToListAsync();

            _dbContext.Exercises.RemoveRange(dummyExercises);
            await _dbContext.SaveChangesAsync();
        }
    }
}