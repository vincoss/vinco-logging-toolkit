using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace Elmah.Everywhere.Appenders.Test
{
    public class MemoryAppenderTest
    {
        [Fact]
        public void Name_Test()
        {
            // Assert
            Assert.Equal("Memory Appender", new MemoryAppender().Name);
        }
    }
}
