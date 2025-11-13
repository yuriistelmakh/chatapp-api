using ChatApp.Api.DTOs;
using ChatApp.Api.Models;

namespace ChatApp.Api.Services.ChatService
{
    public interface IChatService
    {
        Task<IEnumerable<ChatDto>> GetUserChatsAsync(int userId);
        Task<MessageDto> SaveMessageAsync(int chatId, int userId, MessageDto message);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(int chatId);
        Task CreateChatAsync(CreateChatDto dto);
        Task<AddUserToChatDto> AddUserToChat(int chatId, int userId);
    }
}
