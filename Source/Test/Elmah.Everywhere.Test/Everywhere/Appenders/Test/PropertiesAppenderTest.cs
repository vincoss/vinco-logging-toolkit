using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;


namespace Elmah.Everywhere.Appenders.Test
{
    public class PropertiesAppenderTest
    {
        [Fact]
        public void Name_Test()
        {
            // Assert
            Assert.Equal("Properties Appender", new PropertiesAppender().Name);
        }

        [Fact]
        public void Shoult_Append_Properties_Test()
        {
            // Arrange
            PropertiesAppender appender = new PropertiesAppender();
            ErrorInfo error = new ErrorInfo(new Exception("Test-Exception"));
            error.Properties = new Dictionary<string, object>();
            error.Properties.Add("Key", "Value");

            // Act
            appender.Append(error);

            // Act
            Assert.Equal(error.ErrorDetails.Single().Name, appender.Name);
        }
    }
}
