namespace SecretaryTelegramAIBot.Infrastructure.ConfigurationOptions;

public class ThirdPartiesOptions
{
    public TelegramOptions Telegram { get; set; }
    public GenerateAIOptions GenerateAI { get; set; }
    public ConnectionStringsOptions ConnectionStrings { get; set; }
}