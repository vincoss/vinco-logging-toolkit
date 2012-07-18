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
        public void GetError_Throws()
        {
            // Assert
            Assert.Throws<NotSupportedException>(() => { new HttpErrorLog(new HybridDictionary()).GetError(null); });
        }

        [Fact]
        public void GetErrors_Throws()
        {
            // Assert
            Assert.Throws<NotSupportedException>(() => { new HttpErrorLog(new HybridDictionary()).GetErrors(0, 0, null); });
        }

        [Fact]
        public void Log_Throws_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => { new HttpErrorLog(new HybridDictionary()).Log(null); });
        }

        [Fact]
        public void Log_Test()
        {
            // Arrange
            ExceptionDefaults parameters = new ExceptionDefaults
                                               {
                                                   ApplicationName = "Test-Application",
                                                   Host = "HttpErrorLogTest",
                                                   Token = "Test-Token",
                                                   RemoteLogUri = "http://www.faulthub.org/"
                                                   
                                               };
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();
            ExceptionHandler.Configure(writter, parameters);
            HttpErrorLog log = new HttpErrorLog(new HybridDictionary());

            // Act
            string id = log.Log(new Error(new Exception("Test-Error")));

            // Assert
            Assert.True(new Guid(id) != Guid.Empty, id);
        }

        class TestableHttpExceptionWritter : HttpExceptionWritter
        {
            protected override void WriteInternal(string data)
            {
            }
        }
    }
}
