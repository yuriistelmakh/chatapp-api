using ChatApp.Api.DTOs;
using ChatApp.Api.Services.UsersService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            return await _usersService.GetUsersAsync();
        }

        [HttpGet("getFromChat/{chatId}")]
        public async Task<IEnumerable<UserDto>> GetChatUsers(int chatId)
        {
            return await _usersService.GetChatUsers(chatId);
        }
    }
}
