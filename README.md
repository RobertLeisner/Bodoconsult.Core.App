# What does the library

Bodoconsult.Core.App is a library with basic functionality :

1. Single-threaded logging for single-threaded or multi-threaded environments (IAppLoggerProxy / AppLoggerProxy )
2. Basic watchdog implementation IWatchDog / WatchDog as replacement for timers
3. Using PerformanceCounters to log application performance metrics (IPerformanceLogger / PerformanceLogger and IPerformanceLoggerManager / PerformanceLoggerManager) 




# How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

## IAppLoggerProxy / AppLoggerProxy




## IWatchDog / WatchDog

WatchDog is a replacement for timers. Timers do have the potential issue with multiple runnings tasks if tasks may run longer than the timer interval. 

The runner method is fired and processed until done. Then the watchdog waits for the delay interval until it restarts the runner method.

The runner method is running on a separated background thread.


``` csharp

            WatchDogRunnerDelegate runner = Runner;

            var w = new WatchDog(runner, delayTime);
            w.StartWatchDog();
			
			...
			
			w.StopWatchDog();
			
```

``` csharp

        /// <summary>
        /// Runner method for the watchdog
        /// </summary>
        private void Runner()
        {
            // Do your tasks here
        }

```

## IPerformanceLogger / PerformanceLogger


## IPerformanceLoggerManager / PerformanceLoggerManager


# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

