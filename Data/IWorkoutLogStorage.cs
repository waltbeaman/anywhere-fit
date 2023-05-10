namespace AnywhereFit.Data
{
    public interface IWorkoutLogStorage
    {
        Task AddWorkoutAsync(List<Exercise> workout, string userId);
        Task<List<Exercise>> GetAllUserExercisesAsync(string userId);
        Task AddDummyDataAsync(string userId);
        Task DeleteDummyDataAsync(string userId);
        Task<List<WeeklyReps>> GetWeeklyRepsAsync(string userId);
        Task<TotalReps> GetTotalRepsAsync(string userId);
    }
}