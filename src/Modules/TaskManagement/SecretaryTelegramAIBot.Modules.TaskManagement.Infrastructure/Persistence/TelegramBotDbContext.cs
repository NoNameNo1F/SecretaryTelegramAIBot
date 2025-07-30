using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Persistence;
public class TelegramBotDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }

    public TelegramBotDbContext(DbContextOptions<TelegramBotDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public class TelegramBotDbContextFactory : IDesignTimeDbContextFactory<TelegramBotDbContext>
    {
        public TelegramBotDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Hosts/SecretaryTelegramAIBot.API");

            // Load configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var thirdPartiesOptions = new ThirdPartiesOptions();
            configuration.GetSection("ThirdParties").Bind(thirdPartiesOptions);
            var connectionString = thirdPartiesOptions.ConnectionStrings.Default;

            var optionsBuilder = new DbContextOptionsBuilder<TelegramBotDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TelegramBotDbContext(optionsBuilder.Options);
        }
    }
}