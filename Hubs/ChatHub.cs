using ChatApp.Api.DTOs;
using ChatApp.Api.Models;
using ChatApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.ChatHub
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task SendMessageToGroup(int chatId, int userId, MessageDto message)
        {
            var savedMessage = await _chatService.SaveMessageAsync(chatId, userId, message);
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, savedMessage);
        }

        public async Task NewMember(int chatId, int userId)
        {
            var userDto = await _chatService.AddUserToChat(chatId, userId);
            await Clients.All.SendAsync("NewMemberAdded", userDto);
        }
    }
}
