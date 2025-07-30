using Serilog.Events;

namespace SecretaryTelegramAIBot.CrossCuttingConcern.Logging;

public class LoggingOptions
{
    public Dictionary<string, string> LogLevel { get; set; }

    public FileOptions File { get; set; }
}

public class FileOptions
{
    public LogEventLevel MinimumLogEventLevel { get; set; }
}
