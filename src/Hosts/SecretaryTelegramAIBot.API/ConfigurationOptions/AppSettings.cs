using SecretaryTelegramAIBot.CrossCuttingConcern.Logging;
using SecretaryTelegramAIBot.Infrastructure.ConfigurationOptions;
using SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.ConfigurationOptions;

namespace SecretaryTelegramAIBot.API.ConfigurationOptions;

public class AppSettings
{
    public string AllowedHosts { get; set; }
    public LoggingOptions Logging { get; set; }
    public ThirdPartiesOptions ThirdParties { get; set; }
    public GenerativeAIOptions GenerativeAI { get; set; }
}