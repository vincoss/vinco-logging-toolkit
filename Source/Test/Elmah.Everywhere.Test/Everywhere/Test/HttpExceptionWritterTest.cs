using System;
using Xunit;
using System.Web;
using System.Net;


namespace Elmah.Everywhere.Test
{
    public class HttpExceptionWritterTest
    {
        [Fact]
        public void Write_Throws_If_Token_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HttpExceptionWritter().Write(null, new ErrorInfo(new Exception())));
        }

        [Fact]
        public void Write_Throws_If_Error_Is_Null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpExceptionWritter().Write("Test-Token", null));
        }

        [Fact]
        public void Write_Test()
        {
            // Arrange
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();

            ErrorInfo info = new ErrorInfo(new Exception("Test Exception"));

            // Act
            writter.Write("Test-Token", info);

            // Assert
            Assert.NotNull(writter.Data);
        }

        [Fact]
        public void Write_Completed_Event_Called_Test()
        {
            // Arrange
            bool completed = false;
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();

            writter.Completed += (s, e) =>
                                     {
                                         completed = true;
                                     };

            ErrorInfo info = new ErrorInfo(new Exception("Test Exception"));

            // Act
            writter.Write("Test-Token", info);

            // Assert
            Assert.True(completed);
        }

        [Fact]
        public void Write_Exception_Not_Null_If_Error_Occurs_Test()
        {
            HttpExceptionWritter writter = new HttpExceptionWritter();
            ErrorInfo info = new ErrorInfo(new Exception("Test Exception"));

            // Act
            writter.Write("Test-Token", info);

            // Assert
            Assert.NotNull(writter.Exception);
        }

        [Fact]
        public void CanCreateWebClient_Test()
        {
            // Arrange
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();

            WebClient client = writter.GetClient();

            // Assert
            Assert.Equal("application/x-www-form-urlencoded", client.Headers["Content-Type"]);
        }

        class TestableHttpExceptionWritter : HttpExceptionWritter
        {
            public string Data { get; private set; }

            protected override void WriteInternal(string data)
            {
                this.Data = data;
            }

            public WebClient GetClient()
            {
                return CreateWebClient();
            }
        }
    }
}