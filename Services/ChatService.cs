using ChatApp.Api.DTOs;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _context;

        public ChatService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatDto>> GetUserChatsAsync(int userId)
        {
            return await _context.Members
                .Where(cm => cm.UserId == userId)
                .Include(cm => cm.Chat)
                    .ThenInclude(c => c.Messages)
                        .ThenInclude(m => m.Sender)
                .Select(cm => cm.Chat)
                .Select(c => new ChatDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsGroup = c.IsGroup,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int chatId)
        {
            return await _context.Messages.Where(m => m.ChatId == chatId)?
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    SenderName = m.Sender.Username,
                    IsIncoming = false
                })?.ToListAsync() ?? [];
        }

        public async Task<MessageDto> SaveMessageAsync(int chatId, int userId, MessageDto messageDto)
        {
            var message = new Message
            {
                ChatId = chatId,
                SenderId = userId,
                Content = messageDto.Content,
                CreatedAt = messageDto.CreatedAt,
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await _context.Entry(message).Reference(m => m.Sender).LoadAsync();

            return new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                SenderName = message.Sender?.Username ?? "Unknown"
            };
        }

        public async Task<Chat> CreateChatAsync(CreateChatDto dto)
        {
            var members = await _context.Users
                .Where(u => dto.MemberIds.Contains(u.Id))
                .ToListAsync();

            var chat = new Chat
            {
                Name = dto.ChatName,
                IsGroup = dto.MemberIds.Count() > 2,
                CreatedAt = DateTime.UtcNow,
                Members = members.Select(u => new ChatMember
                {
                    UserId = u.Id,
                    JoinedAt = DateTime.UtcNow
                }).ToList()
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return chat;
        }
    }
}
