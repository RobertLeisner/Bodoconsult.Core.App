﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Bodoconsult.Core.App.Exceptions;

namespace Bodoconsult.Core.App.Helpers;

/// <summary>
/// General helper methods
/// </summary>
public static class GeneralHelper
{

    /// <summary>
    /// Delay time in milliseconds for accessing concurrent queues
    /// </summary>
    public static int DelayTimeQueueAccess { get; set; } = 10;

    /// <summary>
    /// Dequeue from a concurrent queue with two retries
    /// </summary>
    /// <typeparam name="T">Type of for the queue</typeparam>
    /// <param name="queue">Current queue</param>
    /// <param name="memberName">Current caller member (leave null normally)</param>
    /// <param name="filepath">Current caller file path (leave null normally)</param>
    /// <param name="lineNumber">Curren caller line number  (leave null normally)</param>
    /// <returns>Dequeued item</returns>
    public static T DequeueFromQueue<T>(ConcurrentQueue<T> queue, 
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filepath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (queue.Count == 0)
        {
            return default;
        }

        var i = 0;

        // ReSharper disable once TooWideLocalVariableScope
        // ReSharper disable once InlineOutVariableDeclaration
        T item;

        while (queue.Count > 0 && i < 10)
        {
            var success = queue.TryDequeue(out item);
            if (success)
            {
                return item;
            }

            i++;
        }

        throw new ConcurrentQueueAccessException($"GeneralHelper.DequeueFromQueue: error dequeuing: {memberName}. File: {filepath} Line: {lineNumber}");

    }

    /// <summary>
    /// Peek from a concurrent queue with two retries
    /// </summary>
    /// <typeparam name="T">Type of for the queue</typeparam>
    /// <param name="queue">Current queue</param>
    /// <param name="memberName">Current caller member (leave null normally)</param>
    /// <param name="filepath">Current caller file path (leave null normally)</param>
    /// <param name="lineNumber">Curren caller line number  (leave null normally)</param>
    /// <returns>Peeked item</returns>
    public static T PeekFromQueue<T>(ConcurrentQueue<T> queue, 
        [CallerMemberName] string memberName = "",  
        [CallerFilePath] string filepath = "", 
        [CallerLineNumber] int lineNumber = 0)
    {

        var i = 0;
        // ReSharper disable once TooWideLocalVariableScope
        // ReSharper disable once InlineOutVariableDeclaration
        T item;

        while (queue.Count > 0 && i < 10)
        {
            var success = queue.TryPeek(out item);
            if (success)
            {
                return item;
            }

            i++;
        }

        throw new ConcurrentQueueAccessException($"GeneralHelper.PeekFromQueue: error peeking: {memberName}. File: {filepath} Line: {lineNumber}");

    }
}