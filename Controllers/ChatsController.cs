using ChatApp.Api.ChatHub;
using ChatApp.Api.DTOs;
using ChatApp.Api.Models;
using ChatApp.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub.ChatHub> _hubContext;

        public ChatsController(IChatService chatService, IHubContext<ChatHub.ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<ChatDto>> GetUsersChatsAsync(int userId)
        {
            return await _chatService.GetUserChatsAsync(userId);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int chatId)
        {
            return await _chatService.GetMessagesAsync(chatId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(CreateChatDto createChatDto)
        {
            var chat = await _chatService.CreateChatAsync(createChatDto);

            var chatDto = new ChatDto 
            { 
                Id = chat.Id, 
                CreatedAt = chat.CreatedAt, 
                IsGroup = chat.IsGroup, 
                Name = chat.Name!
            };

            await _hubContext.Clients.All.SendAsync("ChatCreated", chatDto);

            return Ok(chatDto);
        }
    }
}
