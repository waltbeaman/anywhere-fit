namespace AnywhereFit.Data
{
    public class WeeklyReps
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalReps { get; set; }
        public int UpperBodyReps { get; set; }
        public int LowerBodyReps { get; set; }
        public int CardioReps { get; set; }
    }
}