using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
    {
        // Compose the message and write to console instead of a hardcoded file path.
        string mensagem = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";
        try
        {
            Console.WriteLine(mensagem);
        }
        catch
        {
            // swallow any logging exceptions to avoid breaking application flow
        }
    }

    // Lightweight no-op scope for BeginScope
    private class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        public void Dispose() { }
    }
}
