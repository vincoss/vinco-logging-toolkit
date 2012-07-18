using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace Elmah.Everywhere.Appenders.Test
{
    public class AssemblyAppenderTest
    {
        [Fact]
        public void Name_Test()
        {
            // Assert
            Assert.Equal("AppDomain Assembly Appender", new AssemblyAppender().Name);
        }
    }
}
