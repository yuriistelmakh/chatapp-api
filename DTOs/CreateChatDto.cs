namespace ChatApp.Api.DTOs
{
    public class CreateChatDto
    {
        public string ChatName { get; set; }

        public IEnumerable<int> MemberIds { get; set; } = [];
    }
}
