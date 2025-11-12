using ChatApp.Api.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<ChatMember> Members => Set<ChatMember>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMember>()
                .HasIndex(cm => new { cm.ChatId, cm.UserId })
                .IsUnique();

        }
    }
}
