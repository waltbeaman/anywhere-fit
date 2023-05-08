namespace AnywhereFit.Data
{
    public interface IWorkoutLogStorage
    {
        Task AddWorkoutAsync(List<Exercise> workout, string userId);
        Task<List<Exercise>> GetAllUserExercisesAsync(string userId);
    }
}