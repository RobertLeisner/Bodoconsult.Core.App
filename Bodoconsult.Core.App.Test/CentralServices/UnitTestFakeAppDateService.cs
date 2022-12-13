// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Core.App.CentralServices;

namespace Bodoconsult.Core.App.Test.CentralServices;

[TestFixture]
internal class UnitTestFakeAppDateService
{

    [Test]
    public void TestNow()
    {
        // Arrange 

        var date = DateTime.Now.AddDays(5);
        var service = new FakeAppDateService
        {
            DateTimeToDeliver = date
        };


        // Act  
        var result = service.Now;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.EqualTo(date));

    }

}