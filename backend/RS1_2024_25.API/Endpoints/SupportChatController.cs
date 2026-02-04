using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Services;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MyAuthService _authService;

        public SupportChatController(ApplicationDbContext context, MyAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var authInfo = _authService.GetAuthInfo();
            if (!authInfo.IsAdmin)
            {
                return Unauthorized();
            }

            var conversations = await _context.Conversations
                .Include(c => c.User)
                .Include(c => c.Messages)
                .OrderByDescending(c => c.LastMessageAt)
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    UserName = $"{c.User.Name} {c.User.Surname}",
                   LastMessage = c.Messages.OrderByDescending(m => m.Timestamp).Select(m => m.Content).FirstOrDefault() ?? "",
                    c.LastMessageAt,
                    UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != authInfo.UserId)
                })
                .ToListAsync();

            return Ok(conversations);
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var authInfo = _authService.GetAuthInfo();
            if (!authInfo.IsLoggedIn)
            {
                return Unauthorized();
            }

            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            // Check authorization: either admin or the user who owns the conversation
            if (!authInfo.IsAdmin && conversation.UserId != authInfo.UserId)
            {
                return Forbid();
            }

            var messages = conversation.Messages
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    MessageId = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    SenderName = $"{m.Sender.Name} {m.Sender.Surname}",
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsFromUser = m.SenderId == conversation.UserId,
                    FileUrl = m.FileUrl,
                    FileName = m.FileName
                })
                .ToList();

            return Ok(messages);
        }

        [HttpGet("user-messages")]
        public async Task<IActionResult> GetUserMessages()
        {
            var authInfo = _authService.GetAuthInfo();
            if (!authInfo.IsLoggedIn)
            {
                return Unauthorized();
            }

            // Find user's conversation
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.UserId == authInfo.UserId);

            if (conversation == null)
            {
                // No conversation yet, return empty array
                return Ok(new List<object>());
            }

            var messages = conversation.Messages
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    MessageId = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    SenderName = $"{m.Sender.Name} {m.Sender.Surname}",
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsFromUser = m.SenderId == conversation.UserId,
                    FileUrl = m.FileUrl,
                    FileName = m.FileName
                })
                .ToList();

            return Ok(messages);
        }
    }
}
