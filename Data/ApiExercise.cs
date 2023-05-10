using System.Text.Json.Serialization;

namespace AnywhereFit.Data
{
    public class ApiExercise : IExercise
    {
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
    }
}
