using System;
using System.Linq;
using Xunit;
using Moq;
using Elmah.Everywhere.Configuration;


namespace Elmah.Everywhere.Diagnostics.Test
{
    public class ExceptionHandlerTest
    {
        [Fact]
        public void IsEnabled_Test()
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
        public void Configure_Test()
        {
            // Arrange
            TestableExceptionWritter writter = new TestableExceptionWritter();

            // Act
            ExceptionHandler.ConfigureFromConfigurationFile(writter);
           
            // Assert
            Assert.Equal("http://localhost:11079/error/log", writter.RequestUri.ToString());
        }

        [Fact]
        public void Configure_Thows_If_Section_Is_Null_Test()
        {
            EverywhereConfigurationSection section = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => { ExceptionHandler.Configure(null, section); });
        }

        [Fact]
        public void Configure_Thows_If_Writter_Is_Null_Test()
        {
            HttpExceptionWritter writter = null;
            ExceptionDefaults defaults = new ExceptionDefaults();

            // Assert
            Assert.Throws<ArgumentNullException>(() => { ExceptionHandler.Configure(writter, defaults); });
        }

        [Fact]
        public void Configure_Thows_If_Parameters_Is_Null_Test()
        {
            HttpExceptionWritter writter = new HttpExceptionWritter();
            ExceptionDefaults defaults = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => { ExceptionHandler.Configure(writter, defaults); });
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
            ExceptionDefaults parameters = new ExceptionDefaults
                                               {
                                                   RemoteLogUri = "http://localhost:11079/error/log"
                                               };
            TestableExceptionWritter writter = new TestableExceptionWritter();

            ExceptionHandler.IsEnabled = false;
            ExceptionHandler.Configure(writter, parameters);

            // Act
            ExceptionHandler.Report(new Exception(), null);

            // Reset to default
            ExceptionHandler.IsEnabled = true;

            // Assert
            Assert.Null(writter.Error);
        }

        [Fact]
        public void Should_Report_If_Enabled_Test()
        {
            // Arrange
            ExceptionDefaults parameters = new ExceptionDefaults
                                               {
                                                   Token = "Test-Token",
                                                   RemoteLogUri = "http://localhost:11079/error/log"
                                               };
            TestableExceptionWritter writter = new TestableExceptionWritter();

            ExceptionHandler.Configure(writter, parameters);

            // Act
            ExceptionHandler.Report(new Exception(), null);

            // Assert
            Assert.NotNull(writter.Error);
        }

        [Fact]
        public void Should_Call_Completed_Event_Test()
        {
            // Arrange
            ExceptionDefaults parameters = new ExceptionDefaults
            {
                Token = "Test-Token",
                RemoteLogUri = "http://localhost:11079/error/log"
            };

            bool completed = false;
            TestableExceptionWritter writter = new TestableExceptionWritter();
            writter.Completed += (s, e) =>
            {
                completed = true;
            };

            ExceptionHandler.Configure(writter, parameters);

            // Act
            ExceptionHandler.Report(new Exception(), null);

            // Assert
            Assert.True(completed);
        }

        [Fact]
        public void AllAppenders_Test()
        {
            // Act
            var appenders = ExceptionHandler.AllAppenders();

            // Assert
            Assert.True(appenders.Count() == 5);
        }

        class TestableExceptionWritter : HttpExceptionWritter
        {
            public string Error;
            public Exception SetExcption = null;

            protected override void WriteInternal(string data)
            {
                Error = data;
                Exception = SetExcption;
            }
        }
    }
}
