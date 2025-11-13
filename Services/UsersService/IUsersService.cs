using ChatApp.Api.DTOs;

namespace ChatApp.Api.Services.UsersService
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<IEnumerable<UserDto>> GetChatUsers(int chatId);
    }
}
