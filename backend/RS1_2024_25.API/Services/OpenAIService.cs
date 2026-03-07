using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RS1_2024_25.API.Services;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly LocalAIService _localAI;
    private readonly string _apiKey;

    public OpenAIService(HttpClient httpClient, LocalAIService localAI, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _localAI = localAI;
        _apiKey = configuration["OpenAI:ApiKey"] ?? "";

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


