using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly LocalAIService _localAI;
    private readonly string _apiKey = "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"; // Optional

    public OpenAIService(HttpClient httpClient, LocalAIService localAI)
    {
        _httpClient = httpClient;
        _localAI = localAI;

        if (!string.IsNullOrEmpty(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        try
        {
            var payload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
                new { role = "user", content = question }
            },
                max_tokens = 300
            };

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI API error: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(result);
            return json?.choices[0]?.message?.content?.ToString()
                   ?? "No response from AI";
        }
        catch (Exception ex)
        {
            throw new Exception("OpenAI service failed", ex);
        }
    }
}
