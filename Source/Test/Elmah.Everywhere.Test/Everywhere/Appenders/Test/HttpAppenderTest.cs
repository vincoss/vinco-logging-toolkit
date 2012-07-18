using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Elmah.Everywhere.Appenders.Test
{
    public class HttpAppenderTest
    {
        [Fact]
        public void Name_Test()
        {
            // Assert
            Assert.Equal("Http Appender", new HttpAppender().Name);
        }
    }
}
