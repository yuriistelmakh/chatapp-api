using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Api.Models
{
    public class ChatMember
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Chat))]
        public int ChatId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public virtual Chat Chat { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
