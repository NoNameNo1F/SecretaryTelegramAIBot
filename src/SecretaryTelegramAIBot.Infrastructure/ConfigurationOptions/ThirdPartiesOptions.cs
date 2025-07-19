namespace SecretaryTelegramAIBot.Infrastructure.ConfigurationOptions;

public class ThirdPartiesOptions
{
    public TelegramOptions Telegram { get; set; }
    public GenerativeAIOptions GenerativeAI { get; set; }
    public ConnectionStringsOptions ConnectionStrings { get; set; }
}