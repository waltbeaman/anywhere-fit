namespace AnywhereFit.Data
{
    public class WorkoutLogService
    {
        public async Task AddWorkoutAsync(List<Exercise> workout)
        {
            using (var db = new AnywhereFitContext())
            {
                foreach (var exercise in workout)
                {
                    await db.Exercises.AddAsync(exercise);
                }
                await db.SaveChangesAsync();  
            }
        }

        //public async Task<List<Exercise>> GetExercisesAsync()
    }
}
