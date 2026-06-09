using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class SlowQueryInterceptor : DbCommandInterceptor
{
    private readonly ILogger<SlowQueryInterceptor> _logger;
    private readonly int _thresholdMs;

    public SlowQueryInterceptor(
        ILogger<SlowQueryInterceptor> logger,
        IConfiguration configuration)
    {
        _logger = logger;

        _thresholdMs =
            configuration.GetValue<int?>(
                "SlowQueryThresholdMs")
            ?? 100;
    }

    public override DbDataReader ReaderExecuted( DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        LogIfSlow(command, eventData.Duration);
        return result;
    }

    public override ValueTask<DbDataReader>
    ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData.Duration);
        return ValueTask.FromResult(result);
    }

    //Logging helper
    private void LogIfSlow(DbCommand command, TimeSpan duration)
    {
        if (duration.TotalMilliseconds >
            _thresholdMs)
        {
            _logger.LogWarning(
                "Slow query detected ({Elapsed}ms): {Sql}",
                duration.TotalMilliseconds,
                command.CommandText);
        }
    }
    
}
