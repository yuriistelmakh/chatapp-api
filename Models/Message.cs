using Azure.AI.TextAnalytics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Api.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Chat))]
        public int ChatId { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TextSentiment Sentiment { get; set; } = TextSentiment.Neutral;

        public virtual Chat Chat { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
