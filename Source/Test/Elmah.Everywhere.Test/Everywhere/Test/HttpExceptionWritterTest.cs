using System;
using Xunit;


namespace Elmah.Everywhere.Test
{
    public class HttpExceptionWritterTest
    {
        [Fact]
        public void FormData_Test()
        {
            // Arrange
            ErrorInfo info = new ErrorInfo
                                 {
                                     Token = "1",
                                     ApplicationName = "2",
                                     Host = "3",
                                     Type = "4",
                                     Source = "5",
                                     Message = "6",
                                     Error = "7",
                                     Date = DateTime.MinValue
                                 };

            // Act
            string data = HttpExceptionWritter.FormData(info);

            // Assert
            Assert.Equal<string>("Token=1&ApplicationName=2&Host=3&Type=4&Source=5&Message=6&Error=7&Date=1/01/0001 12:00:00 AM", data);
        }
    }
}