using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        // Get reps by category for last 90 days, broken down by week
        public async Task<List<WeeklyReps>> GetWeeklyRepsAsync(string userId)
        {
            var cutoffDate = DateTime.Today.AddDays(-90);

            var userExercises = await _dbContext.Exercises
                .Where(e => e.UserId == userId && e.DateTime >= cutoffDate).ToListAsync();

            int GetWeekOfYear(DateTime date)
            {
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    date = date.AddDays(3);
                }

                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }

            DateTime FirstDateOfWeek(int year, int weekOfYear)
            {
                var janFirst = new DateTime(year, 1, 1);
                int daysOffset = DayOfWeek.Thursday - janFirst.DayOfWeek;

                DateTime firstThursday = janFirst.AddDays(daysOffset);
                var cal = CultureInfo.CurrentCulture.Calendar;
                int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                var weekNum = weekOfYear;
                if (firstWeek <= 1)
                {
                    weekNum -= 1;
                }

                var result = firstThursday.AddDays(weekNum * 7);

                return result.AddDays(-3);
            }

            var weeklyGroups = userExercises.GroupBy(e => new { Year = e.DateTime.Year, Week = GetWeekOfYear(e.DateTime) });

            var weeklyReps = weeklyGroups.Select(g => new WeeklyReps
            {
                Year = g.Key.Year,
                Week = g.Key.Week,
                StartDate = FirstDateOfWeek(g.Key.Year, g.Key.Week),
                TotalReps = g.Sum(e => e.Reps)
            }).ToList();

            weeklyReps.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

            return weeklyReps ?? new List<WeeklyReps>();
        }


        // Get reps by category for last 90 days
        // Not yet implemented
        public async Task<TotalReps> GetTotalRepsAsync(string userId)
        {
            List<string> upperBody = new List<string> { "back", "chest", "lower arms", "upper arms", "neck", "shoulders" };
            List<string> lowerBody = new List<string> { "lower legs", "upper legs", "waist" };

            var startDate = DateTime.Today.AddDays(-90);

            var totalReps = await _dbContext.Exercises
                .Where(e => e.UserId == userId && e.DateTime >= startDate)
                .GroupBy(e => 1)
                .Select(g => new TotalReps
                {
                    UpperBodyReps = (int)g.Where(e => upperBody.Contains(e.BodyPart)).Sum(e => e.Reps),
                    LowerBodyReps = (int)g.Where(e => lowerBody.Contains(e.BodyPart)).Sum(e => e.Reps),
                    CardioReps = (int)g.Where(e => e.BodyPart == "cardio").Sum(e => e.Reps)
                }).SingleOrDefaultAsync();

            return totalReps ?? new TotalReps();
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