using System;
using Elmah.Everywhere.Handlers;
using Xunit;
using Moq;
using System.Collections.Generic;


namespace Elmah.Everywhere.Diagnostics
{
    public class ExceptionHandlerTest
    {
        [Fact]
        public void IsEnabled_Should_Be_True_Test()
        {
            // Assert
            Assert.True(ExceptionHandler.IsEnabled);
        }

        [Fact]
        public void Should_Report_If_Enabled_Test()
        {
            // Arrange
            Exception exception = new Exception("Test-Exception");
            TestableExceptionWritter writter = new TestableExceptionWritter();
            ExceptionDefaults defaults = new ExceptionDefaults();

            ExceptionHandler.WithParameters(defaults, writter);

            // Act
            ExceptionHandler.Report(exception, null);

            // Assert
            Assert.NotNull(writter.Exception);
            Assert.NotNull(writter.Parameters);
            Assert.NotNull(writter.Properties);
        }

        [Fact]
        public void Should_Not_Report_If_Disabled_Test()
        {
            // Arrange
            Exception exception = new Exception("Test-Exception");
            TestableExceptionWritter writter = new TestableExceptionWritter();
            ExceptionDefaults parameters = new ExceptionDefaults();

            ExceptionHandler.IsEnabled = false;
            ExceptionHandler.WithParameters(parameters, writter);

            // Act
            ExceptionHandler.Report(exception, null);
            ExceptionHandler.IsEnabled = true;

            // Assert
            Assert.Null(writter.Exception);
            Assert.Null(writter.Parameters);
            Assert.Null(writter.Properties);
        }

        class TestableExceptionWritter : ExceptionWritterBase
        {
            public Exception Exception;
            public ExceptionDefaults Parameters;
            public IDictionary<string, object> Properties;

            public override void Write(Exception exception, ExceptionDefaults parameters, IDictionary<string, object> propeties)
            {
                Exception = exception;
                Parameters = parameters;
                Properties = propeties;
            }
        }
    }
}
