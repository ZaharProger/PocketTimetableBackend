using Microsoft.EntityFrameworkCore;
using PocketTimetableBackend.Constants;
using PocketTimetableBackend.Models.Db;

namespace PocketTimetableBackend.Contexts
{
    public class UniversitiesContext: DbContext
    {
        public DbSet<University> Universities { get; set;}
        public DbSet<Group> Groups { get; set;}

        public UniversitiesContext() : base()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile(DbContextConfig.CONFIG_FILE);
            var config = builder.Build();
            string? connectionString = config.GetConnectionString(DbContextConfig.CONNECTION_STRING_KEY);

            optionsBuilder.UseSqlServer(connectionString);           
        }
    }
}
