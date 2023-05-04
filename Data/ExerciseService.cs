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

        public async Task<List<Exercise>> GetCustomExercises()
        {
            var exercises = await GetExercises(); 



            return new List<Exercise>();
        }
    }
}
