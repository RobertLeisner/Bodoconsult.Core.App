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

The IPerformanceLogger implementations read performance counters and provide them as formatted string.

``` csharp

            // Arrange 
            var logger = new PerformanceLogger();

            // Act  
            var s = logger.GetCountersAsString();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Debug.Print(s);

```

## IPerformanceLoggerManager / PerformanceLoggerManager

The IPerformanceLoggerManager implementations are intended to fetch performance counter data from IPerformanceLogger implementations in a scheduled manner 
and provide it as string to a delegate for further usage like logging.

``` csharp

        var logger = new PerformanceLogger();

        var manager = new PerformanceLoggerManager(logger)
        {
            StatusMessageDelegate = StatusMessageDelegate
        };

        Assert.IsNotNull(manager);
        Assert.IsNotNull(manager.PerformanceLogger);

        // Act  
        manager.StartLogging();

```

## IBusinessTransactionManager / IBusinessTransactionManager 

A business transaction is defined here as an external call for a certain functionality of an app by a UI client, webservice or any other client of the app. IBusinessTransactionManager is intended as central point for inbound business transactions for the app.

IBusinessTransactionManager delivers central features like logging and performance measurement for business transaction.

ToDo: add more information

## IExceptionReplyBuilder / ExceptionReplyBuilder

ExceptionReplyBuilder delivers a central exception management to be used standalone or in conjunction with business transactions.

ToDo: add more information

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

