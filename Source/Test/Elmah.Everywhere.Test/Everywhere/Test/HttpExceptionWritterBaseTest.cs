using System;
using System.Web;
using Xunit;


namespace Elmah.Everywhere.Test
{
    public class HttpExceptionWritterBaseTest
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
        public void FormData_Throws_If_Encoder_Is_Null_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                HttpExceptionWritterBase.FormData(null, null);
            });
        }

        [Fact]
        public void FormData_Returns_String_Empty_If_FormValues_Null_Test()
        {
            // Act
            string data = HttpExceptionWritterBase.FormData(null, HttpUtility.UrlEncode);

            // Assert
            Assert.Equal("", data);
        }
    }
}
