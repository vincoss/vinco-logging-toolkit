using System;
using Xunit;


namespace Elmah.Everywhere.Diagnostics.Test
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
            Assert.NotNull(writter.Error);
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
            Assert.Null(writter.Error);
        }

        class TestableExceptionWritter : ExceptionWritterBase
        {
            public ErrorInfo Error;

            protected override void Write(ErrorInfo error)
            {
                Error = error;
            }
        }
    }
}
