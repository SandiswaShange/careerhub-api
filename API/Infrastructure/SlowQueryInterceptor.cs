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
}