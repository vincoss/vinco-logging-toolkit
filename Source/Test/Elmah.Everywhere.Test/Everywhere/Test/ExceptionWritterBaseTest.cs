using System;
using Xunit;
using System.Collections.Generic;


namespace Elmah.Everywhere.Test
{
    public class ExceptionWritterBaseTest
    {
        [Fact]
        public void Report_Test()
        {
            // Arrange
            TestableExceptionWritterBase writter = new TestableExceptionWritterBase();

            Exception exception = new Exception("Test-Exception");

            ExceptionDefaults defaults = new ExceptionDefaults();
            defaults.Token = "Token";
            defaults.Host = "Host";
            defaults.ApplicationName = "ApplicationName";

            IDictionary<string, object> propeties = new Dictionary<string, object>();

            string dumpReport = "Report";

            // Act
            writter.Report(exception, defaults, propeties, dumpReport);

            // Assert
            Assert.NotNull(writter.Error);
            Assert.Equal("Token", writter.Error.Token);
            Assert.Equal("Host", writter.Error.Host);
            Assert.Equal("ApplicationName", writter.Error.ApplicationName);
            Assert.Equal(exception.GetType().FullName, writter.Error.Type);
            Assert.Equal(exception.Source, writter.Error.Source);
            Assert.Equal(exception.Message, writter.Error.Message);
            Assert.NotEmpty(writter.Error.Error);
            Assert.Equal(DateTime.Now.ToShortDateString(), writter.Error.Date.ToShortDateString());
        }

        class TestableExceptionWritterBase : ExceptionWritterBase
        {
            public ErrorInfo Error;

            public override void Write(ErrorInfo error)
            {
                Error = error;
            }
        }
    }
}
