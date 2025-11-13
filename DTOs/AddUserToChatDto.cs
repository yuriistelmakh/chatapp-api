namespace ChatApp.Api.DTOs
{
    public class AddUserToChatDto
    {
        public ChatDto Chat { get; set; }

        public UserDto User { get; set; }
    }
}
