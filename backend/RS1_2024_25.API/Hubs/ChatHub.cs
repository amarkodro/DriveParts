using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.Security.Claims;

namespace RS1_2024_25.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                await base.OnConnectedAsync();
                return;
            }

            var user = await _context.UserAccounts.FindAsync(userId);
            if (user == null)
            {
                await base.OnConnectedAsync();
                return;
            }

            // Add user to their personal group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");

            // If admin, add to Admins group
            if (user.IsAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        public async Task SendMessageToAdmins(string content)
        {
            var userId = GetUserId();
            if (userId == null) return;

            // Find or create conversation
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.UserId == userId.Value);

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    UserId = userId.Value,
                    StartedAt = DateTime.UtcNow,
                    LastMessageAt = DateTime.UtcNow
                };
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }
            else
            {
                conversation.LastMessageAt = DateTime.UtcNow;
            }

            // Save message
            var message = new Message
            {
                ConversationId = conversation.Id,
                SenderId = userId.Value,
                Content = content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Get sender info
            var sender = await _context.UserAccounts.FindAsync(userId.Value);

            // Broadcast to all admins
            await Clients.Group("Admins").SendAsync("ReceiveMessage", new
            {
                messageId = message.Id,
                conversationId = conversation.Id,
                senderId = userId.Value,
                senderName = $"{sender?.Name} {sender?.Surname}",
                content = content,
                timestamp = message.Timestamp,
                isFromUser = true
            });
        }

        public async Task SendMessageToUser(int targetUserId, string content)
        {
            var adminId = GetUserId();
            if (adminId == null) return;

            var admin = await _context.UserAccounts.FindAsync(adminId.Value);
            if (admin == null || !admin.IsAdmin) return;

            // Find conversation
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.UserId == targetUserId);

            if (conversation == null) return;

            conversation.LastMessageAt = DateTime.UtcNow;

            // Save message
            var message = new Message
            {
                ConversationId = conversation.Id,
                SenderId = adminId.Value,
                Content = content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Send to specific user
            await Clients.Group($"User_{targetUserId}").SendAsync("ReceiveMessage", new
            {
                messageId = message.Id,
                conversationId = conversation.Id,
                senderId = adminId.Value,
                senderName = $"{admin.Name} {admin.Surname}",
                content = content,
                timestamp = message.Timestamp,
                isFromUser = false
            });

            // Also notify other admins
            await Clients.Group("Admins").SendAsync("ReceiveMessage", new
            {
                messageId = message.Id,
                conversationId = conversation.Id,
                senderId = adminId.Value,
                senderName = $"{admin.Name} {admin.Surname}",
                content = content,
                timestamp = message.Timestamp,
                isFromUser = false,
                targetUserId = targetUserId
            });
        }

        public async Task MarkMessagesAsRead(int conversationId)
        {
            var userId = GetUserId();
            if (userId == null) return;

            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId && !m.IsRead && m.SenderId != userId.Value)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        private int? GetUserId()
        {
            var user = Context.User;
            if (user == null) return null;

            // Try standard NameIdentifier
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            // Try "id" claim (used in AuthController)
            userIdClaim = user.FindFirst("id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
            {
                return userId;
            }

            // Try "sub" claim
            userIdClaim = user.FindFirst("sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
            {
                return userId;
            }
            
            return null;
        }
    }
}
