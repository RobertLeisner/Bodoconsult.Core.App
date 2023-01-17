using Bodoconsult.Core.App.BufferPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodoconsult.Core.App.Test.BufferPool;

internal class UnitTestBufferPool
{
    private const int NumberOfItems = 1000;

    [Test]
    public void TestAllocate()
    {
        // Arrange 
        var myPool = new BufferPool<byte[]>(() => new byte[65535]);

        // Act  
        myPool.Allocate(NumberOfItems);

        // Assert
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));

    }

    [Test]
    public void TestDequeue()
    {
        // Arrange 
        var myPool = new BufferPool<byte[]>(() => new byte[65535]);
        myPool.Allocate(NumberOfItems);

        // Act  
        var buffer = myPool.Dequeue();

        // Assert
        Assert.IsNotNull(buffer);
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems - 1));


    }


    [Test]
    public void TestEnqueue()
    {
        // Arrange 
        var myPool = new BufferPool<byte[]>(() => new byte[65535]);
        myPool.Allocate(1000);

        var buffer = myPool.Dequeue();

        // Act  
        myPool.Enqueue(buffer);

        // Assert
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));

    }

}