namespace ChatApp.Api.DTOs
{
    public class CreateChatDto
    {
        public ChatDto Chat { get; set; }

        public IEnumerable<int> MemberIds { get; set; } = [];
    }
}
