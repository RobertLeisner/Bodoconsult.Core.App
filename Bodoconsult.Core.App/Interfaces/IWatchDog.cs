// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Interfaces
{
    /// <summary>
    /// Delegate fired by <see cref="IWatchDog"/> implementations
    /// </summary>
    public delegate void WatchDogRunnerDelegate();

    /// <summary>
    /// Interface for watchdog implementations
    /// </summary>
    public interface IWatchDog
    {
        /// <summary>
        /// The method to run by the watchdog
        /// </summary>
        WatchDogRunnerDelegate WatchDogRunnerDelegate { get; }

        /// <summary>
        /// Is the watchdog activated? If yes, <see cref="WatchDogRunnerDelegate"/> is called.
        /// If no the <see cref="WatchDogRunnerDelegate"/> is NOT called.
        /// </summary>
        bool IsActivated { get; set; }


        /// <summary>
        /// The delay after the runner method was running in milliseconds
        /// </summary>
        int DelayUntilNextRunnerFired{ get; set; }


        /// <summary>
        /// Start the watchdog
        /// </summary>
        void StartWatchDog();


        /// <summary>
        /// Stop the watchdog
        /// </summary>
        void StopWatchDog();


    }
}