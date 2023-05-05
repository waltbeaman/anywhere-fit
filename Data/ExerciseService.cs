using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace AnywhereFit.Data
{
    public class ExerciseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ExerciseService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Exercise>> GetExercises()
        {
            // Build header & load RapidAPI Key
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _config["x-rapidapi-key"]);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "exercisedb.p.rapidapi.com");

            // Get only body weight exercises from database
            var response = await _httpClient.GetAsync("https://exercisedb.p.rapidapi.com/exercises/equipment/body%20weight");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            
            // Parse JSON objects into cleaner Exercise objects
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(body);

            //var refinedExercises = exercises.Where(e => e.Equipment.Equals("body weight")).ToList();

            return exercises;

        }

        public async Task<List<Exercise>> GetExercisesByType(string workoutType, int numExercises)
        {
            List<Exercise> exercisesByType = new List<Exercise>();

            List<string> upperBody = new List<string> { "back", "chest", "lower arms", "upper arms", "neck", "shoulders" };
            List<string> lowerBody = new List<string> { "lower legs", "upper legs", "waist" };

            var exercises = await GetExercises();

            if (workoutType == "Upper Body")
            {
                exercisesByType = exercises.Where(e => upperBody.Contains(e.BodyPart)).ToList();
            }
            else if (workoutType == "Lower Body")
            {
                exercisesByType = exercises.Where(e => lowerBody.Contains(e.BodyPart)).ToList();
            }
            else if (workoutType == "Full Body")
            {
                exercisesByType = exercises;
            }
            else // Cardio
            {
                exercisesByType = exercises.Where(e => e.BodyPart.Equals("cardio")).ToList();
            }

            Random random = new Random();
            var results = exercisesByType.OrderBy(e => random.Next()).Take(numExercises).ToList();

            return results;
        }
    }
}
