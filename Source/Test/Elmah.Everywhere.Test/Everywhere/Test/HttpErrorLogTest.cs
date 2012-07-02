using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xunit;
using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere.Test
{
    public class HttpErrorLogTest
    {
        [Fact]
        public void Name_Test()
        {
            // Arrange
            HttpErrorLog log = new HttpErrorLog(new HybridDictionary());

            // Assert
            Assert.Equal("Http Error Log", log.Name);
        }

        [Fact]
        public void Log_Test()
        {
            // Arrange
            ExceptionDefaults parameters = new ExceptionDefaults
                                               {
                                                   ApplicationName = "Test-Application",
                                                   Host = "HttpErrorLogTest",
                                                   Token = "Test-Token"
                                               };
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();
            ExceptionHandler.WithParameters(parameters, writter);
            HttpErrorLog log = new HttpErrorLog(new HybridDictionary());

            // Act
            string id = log.Log(new Error(new Exception("Test-Error")));

            // Assert
            Assert.True(new Guid(id) != Guid.Empty, id);
        }

        class TestableHttpExceptionWritter : HttpExceptionWritter
        {
        }
    }
}
