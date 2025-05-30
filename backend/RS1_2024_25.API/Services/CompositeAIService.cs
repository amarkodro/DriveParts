using System.Threading.Tasks;

public class CompositeAIService
{
    private readonly OpenAIService _openAIService;
    private readonly LocalAIService _localAIService;
    private bool _useFallback = false;

    public CompositeAIService(OpenAIService openAIService, LocalAIService localAIService)
    {
        _openAIService = openAIService;
        _localAIService = localAIService;
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        // If we're in fallback mode, use local AI immediately
        if (_useFallback)
        {
            return await _localAIService.AskQuestionAsync(question);
        }

        try
        {
            // First try OpenAI
            var response = await _openAIService.AskQuestionAsync(question);

            // Reset fallback if successful after failure
            _useFallback = false;
            return response;
        }
        catch
        {
            // Switch to fallback mode
            _useFallback = true;
            return await _localAIService.AskQuestionAsync(question);
        }
    }

    // Method to check current AI status
    public string GetCurrentAIStatus()
    {
        return _useFallback
            ? "Using local AI (Ollama)"
            : "Using OpenAI";
    }
}