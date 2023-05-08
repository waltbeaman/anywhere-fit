using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AnywhereFit.Data
{
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("bodyPart")]
        public string BodyPart { get; set; } = null!;

        [JsonPropertyName("target")]
        public string TargetMuscle { get; set; } = null!;

        [JsonPropertyName("gifUrl")]
        public string? GifUrl { get; set; }

        [JsonPropertyName("equipment")]
        public string? Equipment { get; set; }

        public int? Reps { get; set; }

        public DateTime? DateTime { get; set; }

        public string? UserId { get; set; }

    }
}
