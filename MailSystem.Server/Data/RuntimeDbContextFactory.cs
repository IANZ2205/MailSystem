using Microsoft.EntityFrameworkCore;

namespace MailSystem.Server.Data
{
    public class RuntimeDbContextFactory : IDbContextFactory<MailSystemDbContext>
    {
        // Build the options ONCE and reuse them for every connection
        private static readonly DbContextOptions<MailSystemDbContext> _options;

        static RuntimeDbContextFactory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MailSystemDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MailSystemDb;Trusted_Connection=True;");
            _options = optionsBuilder.Options;
        }

        public MailSystemDbContext CreateDbContext()
        {
            return new MailSystemDbContext(_options);
        }
    }
}