using System;
using Xunit;
using System.Web;


namespace Elmah.Everywhere.Test
{
    public class HttpExceptionWritterTest
    {
        [Fact]
        public void FormData_Test()
        {
            // Arrange
            var entity = new { first = "value 1", second = "value 2" };

            // Act
            string data = HttpExceptionWritterBase.FormData(entity, HttpUtility.UrlEncode);

            // Assert
            Assert.Equal("first=value+1&second=value+2", data);
        }

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
            TestableHttpExceptionWritter writter = new TestableHttpExceptionWritter();
            ErrorInfo info = new ErrorInfo(new Exception("Test Exception"));
            writter.Throw = true;

            // Act
            writter.Write("Test-Token", info);

            // Assert
            Assert.NotNull(writter.Exception);
        }

        class TestableHttpExceptionWritter : HttpExceptionWritter
        {
            public bool Throw { get; set; }
            public string Data { get; private set; }
        }
    }
}