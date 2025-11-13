using ChatApp.Api.DTOs;
using ChatApp.Api.Exceptions;
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

        public async Task CreateChatAsync(CreateChatDto dto)
        {
            var members = await _context.Users
                .Where(u => dto.MemberIds.Contains(u.Id))
                .ToListAsync();

            var chat = new Chat
            {
                Name = dto.Chat.Name,
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

            dto.Chat.Id = chat.Id;
        }

        public async Task<AddUserToChatDto> AddUserToChat(int chatId, int userId)
        {
            var chat = await _context.Chats
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                throw new NotFoundException($"Chat with ID {chatId} not found.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            if (chat.Members.Any(m => m.UserId == userId))
                throw new InvalidOperationException("User is already a member of this chat.");

            chat.Members.Add(new ChatMember
            {
                UserId = userId,
                ChatId = chatId
            });

            await _context.SaveChangesAsync();

            var chatDto = new ChatDto
            {
                Id = chat.Id,
                Name = chat.Name,
                IsGroup = chat.IsGroup,
                CreatedAt = chat.CreatedAt
            };

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };

            var addUserToChatDto = new AddUserToChatDto()
            {
                Chat = chatDto,
                User = userDto
            };

            return addUserToChatDto;
        }

    }
}
