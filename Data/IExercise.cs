namespace AnywhereFit.Data
{
    public interface IExercise
    {
        string BodyPart { get; set; }
        string Name { get; set; }
        string TargetMuscle { get; set; }
    }
}