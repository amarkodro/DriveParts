using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Services;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupportChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("conversations")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetConversations()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var adminId)) return Unauthorized();

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
                    UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != adminId)
                })
                .ToListAsync();

            return Ok(conversations);
        }

        [HttpGet("messages/{conversationId}")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();
            
            bool isAdmin = User.IsInRole("Admin");

            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            // Check authorization: either admin or the user who owns the conversation
            if (!isAdmin && conversation.UserId != userId)
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
        [Authorize]
        public async Task<IActionResult> GetUserMessages()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            // Find user's conversation
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.UserId == userId);

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

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (conversation == null)
            {
                return Ok(new { count = 0 });
            }

            var count = conversation.Messages.Count(m => !m.IsRead && m.SenderId != userId);
            return Ok(new { count });
        }
    }
}
