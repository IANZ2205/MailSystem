using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data.Entities;
using MessageEntity = MailSystem.Server.Data.Entities.Message;
using AttachmentEntity = MailSystem.Server.Data.Entities.Attachment;

namespace MailSystem.Server.Data
{
    public class MailSystemDbContext : DbContext
    {
        public MailSystemDbContext(DbContextOptions<MailSystemDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<MessageRecipient> MessageRecipients { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<DeliveryLog> DeliveryLogs { get; set; }
        public DbSet<SpamScore> SpamScores { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ServerLog> ServerLogs { get; set; }
        public DbSet<SpamRule> SpamRules { get; set; }  // NEW

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AttachmentEntity>().HasKey(a => a.Id);
            modelBuilder.Entity<DeliveryLog>().HasKey(d => d.Id);
            modelBuilder.Entity<SpamScore>().HasKey(s => s.Id);
            modelBuilder.Entity<ServerLog>().HasKey(s => s.Id);
            modelBuilder.Entity<MessageRecipient>().HasKey(m => m.Id);
            modelBuilder.Entity<SpamRule>().HasKey(r => r.Id);  // NEW

            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();
            modelBuilder.Entity<Folder>().HasIndex(f => new { f.UserId, f.Name }).IsUnique();

            // NEW: One whitelist/blacklist entry per (owner, sender) pair
            modelBuilder.Entity<SpamRule>().HasIndex(r => new { r.OwnerId, r.SenderEmail }).IsUnique();

            modelBuilder.Entity<MessageEntity>()
                .HasOne<User>().WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageRecipient>()
                .HasOne<User>().WithMany().HasForeignKey(mr => mr.RecipientId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne<User>().WithMany().HasForeignKey(c => c.OwnerId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne<User>().WithMany().HasForeignKey(c => c.ContactUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}