using Microsoft.AspNetCore.Mvc;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly CompositeAIService _aiService;

        public ChatController(CompositeAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] QuestionDto dto)
        {
            var answer = await _aiService.AskQuestionAsync(dto.Question);
            return Ok(new
            {
                answer,
                aiStatus = _aiService.GetCurrentAIStatus()
            });
        }
    }

    public class QuestionDto
    {
        public string Question { get; set; }
    }
}
