// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Concurrent;

namespace Bodoconsult.Core.App.BufferPool;

/// <summary>
/// Buffer pool is used to store reusable objects to reduce GC pressure
/// </summary>
/// <typeparam name="T">Type of the object class to store</typeparam>
public class BufferPool<T>
{
    private readonly Func<T> _factoryMethod;
    private readonly ConcurrentQueue<T> _queue = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="factoryMethod">Factory method for object creation</param>
    public BufferPool(Func<T> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    /// <summary>
    /// The current length of the internal queue
    /// </summary>
    public int LengthOfQueue => _queue.Count;


    /// <summary>
    /// Pre-allocate a certain number of objects stored in the pool
    /// </summary>
    /// <param name="numberOfInstances">Number of objects stored in the pool</param>
    public void Allocate(int numberOfInstances)
    {
        for (var i = 0; i < numberOfInstances; i++)
        {
            _queue.Enqueue(_factoryMethod());
        }
    }

    /// <summary>
    /// Dequeue an object to use from buffer pool
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        return !_queue.TryDequeue(out var buffer) ? _factoryMethod() : buffer;
    }

    /// <summary>
    /// Queue an used oject back to the buffer pool
    /// </summary>
    /// <param name="buffer">Resuable object to store in the pool</param>
    public void Enqueue(T buffer)
    {
        _queue.Enqueue(buffer);
    }
}