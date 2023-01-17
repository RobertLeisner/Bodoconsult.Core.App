// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.Helpers;

/// <summary>
/// Default implementation of <see cref="IWatchDog"/>
/// </summary>
public class WatchDog : IWatchDog
{

    private CancellationToken _cancellationToken = new(false);

    private Thread _watchDogThread;

    private readonly ThreadPriority _threadPriority;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="watchDogRunnerDelegate"></param>
    /// <param name="delayUntilNextRunnerFired">Delay until next run</param>
    public WatchDog(WatchDogRunnerDelegate watchDogRunnerDelegate, int delayUntilNextRunnerFired)
    {
        WatchDogRunnerDelegate = watchDogRunnerDelegate ?? throw new ArgumentNullException(nameof(watchDogRunnerDelegate));
        DelayUntilNextRunnerFired = delayUntilNextRunnerFired;
        _threadPriority = ThreadPriority.Normal;
    }

    /// <summary>
    /// Ctor with additional thread priority setting
    /// </summary>
    /// <param name="watchDogRunnerDelegate"></param>
    /// <param name="delayUntilNextRunnerFired">Delay until next run</param>
    /// <param name="threadPriority">Thread priority</param>
    public WatchDog(WatchDogRunnerDelegate watchDogRunnerDelegate, int delayUntilNextRunnerFired, ThreadPriority threadPriority)
    {
        WatchDogRunnerDelegate = watchDogRunnerDelegate;
        DelayUntilNextRunnerFired = delayUntilNextRunnerFired;
        _threadPriority = threadPriority;
    }

    /// <summary>
    /// The method to run by the watchdog
    /// </summary>
    public WatchDogRunnerDelegate WatchDogRunnerDelegate { get; }

    /// <summary>
    /// Is the watchdog activated? If yes, <see cref="IWatchDog.WatchDogRunnerDelegate"/> is called.
    /// If no the <see cref="IWatchDog.WatchDogRunnerDelegate"/> is NOT called.
    /// </summary>
    public bool IsActivated { get; set; } = true;

    /// <summary>
    /// The delay after the runner method was running in milliseconds
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; }

    /// <summary>
    /// Start the watchdog
    /// </summary>
    public void StartWatchDog()
    {
        _cancellationToken = new CancellationToken(false);
        _watchDogThread = new Thread(RunInternal)
        {
            Priority = _threadPriority,
            IsBackground = true
        };
        _watchDogThread.Start();
    }

    /// <summary>
    /// Run the watchdog
    /// </summary>
    public void RunInternal()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {

            if (IsActivated)
            {
                //Run the delegate
                WatchDogRunnerDelegate?.Invoke();
            }

            // Proceed only if not cancelled
            if (_cancellationToken.IsCancellationRequested)
            {
                break;
            }

            // Delay the thread as requested
            Thread.Sleep(DelayUntilNextRunnerFired);

        }

        WatchDogRunnerDelegate?.Invoke();

    }

    /// <summary>
    /// Stop the watchdog
    /// </summary>
    public void StopWatchDog()
    {
        _cancellationToken = new CancellationToken(true);

        _watchDogThread = null;

    }
}