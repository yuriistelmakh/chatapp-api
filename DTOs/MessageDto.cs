using Azure.AI.TextAnalytics;

namespace ChatApp.Api.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public bool IsIncoming { get; set; }
        public TextSentiment Sentiment { get; set; }
    }
}