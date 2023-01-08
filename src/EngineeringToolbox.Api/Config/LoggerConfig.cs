using Serilog.Core;
using Serilog.Events;
using Serilog;

namespace EngineeringToolbox.Api.Config
{
    public static class LoggerConfig
    {
        private static readonly LogEventLevel _defaultLogLevel = LogEventLevel.Information;
        private static readonly LoggingLevelSwitch _loggingLevel = new LoggingLevelSwitch();
        public static void AddLoggerConfig(this IConfiguration configuration)
        {
            LoadLogLevel(configuration);
            ConfigureLog();
        }

        private static void ConfigureLog()
        {
            Log.Logger = new LoggerConfiguration()
                  .WriteTo.Console()
                  .CreateLogger();
        }

        private static void LoadLogLevel(IConfiguration configuration)
        {
            var configLogLevel = configuration.GetSection("Logger").GetSection("LogLevel")["Default"];
            bool parsed = Enum.TryParse(configLogLevel, true, out LogEventLevel logLevel);

            _loggingLevel.MinimumLevel = parsed ? logLevel : _defaultLogLevel;
        }
    }
}