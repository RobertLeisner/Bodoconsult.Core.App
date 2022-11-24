using Bodoconsult.Core.App.Delegates;

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for performance logging manager implementations
/// </summary>
public interface IPerformanceLoggerManager
{
    /// <summary>
    /// The delay after the runner method was running in milliseconds
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; }

    /// <summary>
    /// Current status message delegate to be called from the <see cref="Log"/> method
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; set; }


    /// <summary>
    /// Start the performance logging
    /// </summary>
    void StartLogging();

    /// <summary>
    /// Stop the performance logging
    /// </summary>
    void StopLogging();


    /// <summary>
    /// Log the performance data
    /// </summary>
    void Log();


}