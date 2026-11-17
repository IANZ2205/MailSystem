using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MailSystem.Server.Data
{
    public class MailSystemDbContextFactory : IDesignTimeDbContextFactory<MailSystemDbContext>
    {
        // This connection string is needed by EF Core to run the migration commands.
        // The actual server will use its own factory later.
        public MailSystemDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MailSystemDbContext>();

            // We use LocalDB. (You can change this name later in the server settings)
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MailSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new MailSystemDbContext(optionsBuilder.Options);
        }
    }
}