// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.Helpers;

public static class Wait
{
    //Mininum time in mSec waited before execution
    public static int MinIntervalMills => 1;

    //Maximum time in mSec waited before next execution
    public static int MaxIntervalMills => 100;

    //Maxium time to be waited before timeout
    public static int TimeoutInMilliseconds => 1000 * 5;

    /// <summary>
    /// The delay time between a check run in <see cref="Until"/>
    /// </summary>
    public static int DelayTime { get; set; } = 5;

    //public const int TimeoutInMills = 60 * 1000 * 30;

    //public static TimeSpan Timeout => TimeSpan.FromMilliseconds(TimeoutInMills);


    /// <summary>
    /// This method execute any commands wrapped as a Predicate periodically until it is timeout.
    /// If the command execution is success (when predicate returns true), then it would return immediately.
    /// Otherwise, all exepections except the last one due to the command execution would be discarded considering
    /// that they are identical; and the last exception would be throw when command execution is not success when
    /// timeout happens.
    /// </summary>
    /// <param name="predicate">Wrapper of the execution codes that might throw exceptions.</param>
    /// <param name="timeoutMilliseconds">Time waited before draw conclusion that the command cannot succeed in milliseconds.</param>
    /// <returns>'true' if returns before timeout, 'false' when timeout happened.</returns>
    public static bool Until(Func<bool> predicate, int timeoutMilliseconds = 5000)
    {
        if (timeoutMilliseconds <= 0)
        {
            throw new System.ComponentModel.InvalidEnumArgumentException("The timeout must be a positive value");
        }

        var current = 0;
        while (current <= timeoutMilliseconds)
        {
            if (predicate())
            {
                return true;
            }

            Thread.Sleep(DelayTime);

            current += DelayTime;
        }

        return false;

    }











    /// <summary>
    /// Blocks while condition is true or timeout occurs.
    /// </summary>
    /// <param name="condition">The condition that will perpetuate the block.</param>
    /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <exception cref="TimeoutException"></exception>
    /// <returns></returns>
    public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
    {
        var waitTask = Task.Run(async () =>
        {
            while (condition())
            {
                await Task.Delay(frequency);
            }
        });

        if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
        {
            throw new TimeoutException();
        }
    }

    /// <summary>
    /// Blocks until condition is true or timeout occurs.
    /// </summary>
    /// <param name="condition">The break condition.</param>
    /// <param name="frequency">The frequency at which the condition will be checked.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    /// <returns></returns>
    public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
    {
        while (!condition())
        {
            await Task.Delay(frequency);
        }

        //while (!condition())
        //{
        //    Task.Delay(frequency).Wait();
        //}

        /*if (waitTask != await Task.WhenAny(waitTask,
        Task.Delay(timeout)))
        throw new TimeoutException();*/
    }

}