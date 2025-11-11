using System.ComponentModel.DataAnnotations;

namespace ChatApp.Api.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public bool IsGroup { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ChatMember> Members { get; set; } = [];
        public virtual ICollection<Message> Messages { get; set; } = [];
    }
}
