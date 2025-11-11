using System.ComponentModel.DataAnnotations;

namespace ChatApp.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ChatMember> ChatMembers { get; set; } = [];
        public virtual ICollection<Message> Messages { get; set; } = [];
    }
}
