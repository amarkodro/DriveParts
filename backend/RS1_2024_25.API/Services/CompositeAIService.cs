using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Might be needed for consistency, but checking dependencies

namespace RS1_2024_25.API.Services;

public class CompositeAIService
{
    private readonly OpenAIService _openAIService;
    private readonly LocalAIService _localAIService;
    private static bool _useFallback = false;
    private static DateTime _fallbackUntil = DateTime.MinValue;
    private static readonly TimeSpan FallbackDuration = TimeSpan.FromMinutes(5);

    public CompositeAIService(OpenAIService openAIService, LocalAIService localAIService)
    {
        _openAIService = openAIService;
        _localAIService = localAIService;
    }

    public async Task<string> AskQuestionAsync(string question)
    {

        if (_useFallback && DateTime.UtcNow < _fallbackUntil)
            return await _localAIService.AskQuestionAsync(question);


        _useFallback = false;

        try
        {
            var response = await _openAIService.AskQuestionAsync(question);
            return response;
        }
        catch
        {

            _useFallback = true;
            _fallbackUntil = DateTime.UtcNow.Add(FallbackDuration);
            return await _localAIService.AskQuestionAsync(question);
        }
    }

    public string GetCurrentAIStatus()
    {
        return _useFallback && DateTime.UtcNow < _fallbackUntil
            ? "Using local AI (Ollama)"
            : "Using OpenAI";
    }
}
