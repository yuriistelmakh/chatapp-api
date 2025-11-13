using ChatApp.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services
{
    public class UsersService : IUsersService
    {
        private readonly ChatDbContext _context;

        public UsersService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt
            }).ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetChatUsers(int chatId)
        {
            var chat = await _context.Chats.Include(c => c.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            return chat.Members.Select(m => new UserDto
            {
                Id = m.User.Id,
                Username = m.User.Username,
                CreatedAt = m.User.CreatedAt,
            });
        }
    }
}
