using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class LocalAIService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<string> AskQuestionAsync(string question)
    {
        try
        {
            var payload = new
            {
                model = "phi3",
                prompt = question,
                stream = false
            };

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            if (!response.IsSuccessStatusCode)
            {
                return $"Ollama error: {response.StatusCode}";
            }

            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(result);
            return json?.response?.ToString()?.Trim()
                   ?? "No response from local AI";
        }
        catch
        {
            return "Local AI service unavailable";
        }
    }
}