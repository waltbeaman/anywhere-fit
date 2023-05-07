using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnywhereFit.Data
{
    public class Exercise
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

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

    }
}
