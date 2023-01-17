// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.Core.App.CentralServices;

namespace Bodoconsult.Core.App.Test.CentralServices;

[TestFixture]
internal class UnitTestAppDateService
{

    [Test]
    public void TestNow()
    {
        // Arrange 
        var service = new AppDateService();

        // Act  
        var result = service.Now;

        // Assert
        Assert.IsNotNull(result);

    }
        
}