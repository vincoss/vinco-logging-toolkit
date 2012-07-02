using System;
using Xunit;
using Moq;


namespace Elmah.Everywhere.Diagnostics.Test
{
    // TODO:
    public class ExceptionHandlerTest
    {
        public ExceptionHandlerTest()
        {
            ExceptionHandler.IsEnabled = true;
        }

        [Fact]
        public void IsEnabled_Should_Be_True_Test()
        {
            // Assert
            Assert.True(ExceptionHandler.IsEnabled);
        }

        [Fact]
        public void Attach_Throws_If_Domain_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.Attach(null));
        }

        [Fact]
        public void Detach_Throws_If_Domain_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.Detach(null));
        }

        [Fact]
        public void WithParameters_Throws_If_Parameters_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.WithParameters(null, new HttpExceptionWritter()));
        }

        [Fact]
        public void WithParameters_Throws_If_Writer_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.WithParameters(new ExceptionDefaults(), null));
        }

        [Fact]
        public void Report_Throws_If_Exception_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.Report(null));
        }

        [Fact]
        public void Should_Not_Report_If_Disabled_Test()
        {
            // Arrange
            ExceptionDefaults parameters = new ExceptionDefaults();
            TestableExceptionWritter writter = new TestableExceptionWritter();

            ExceptionHandler.IsEnabled = false;
            ExceptionHandler.WithParameters(parameters, writter);

            // Act
            ExceptionHandler.Report(new Exception(), null);

            // Assert
            Assert.Null(writter.Error);
        }

        [Fact]
        public void Should_Report_If_Enabled_Test()
        {
            // Arrange
            ExceptionDefaults defaults = new ExceptionDefaults();
            TestableExceptionWritter writter = new TestableExceptionWritter();

            ExceptionHandler.WithParameters(defaults, writter);

            // Act
            ExceptionHandler.Report(new Exception(), null);

            // Assert
            Assert.NotNull(writter.Error);
        }

        class TestableExceptionWritter : ExceptionWritterBase
        {
            public ErrorInfo Error;

            public override void Write(string token, ErrorInfo error)
            {
                Error = error;
            }
        }
    }
}
