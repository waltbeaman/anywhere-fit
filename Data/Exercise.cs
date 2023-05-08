using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AnywhereFit.Data
{
    public class Exercise : IExercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string BodyPart { get; set; } = null!;
        public string TargetMuscle { get; set; } = null!;
        public string? Equipment { get; set; }
        public int? Reps { get; set; }
        public DateTime? DateTime { get; set; }
        public string? UserId { get; set; }
    }
}
