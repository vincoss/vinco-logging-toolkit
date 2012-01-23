using System;
using Xunit;


namespace Elmah.Everywhere.Test
{
    // TODO: Fix me proplem with ID
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
            Assert.Equal<string>("Id=de9a9d10-0ec5-4b18-8781-8b0854d3361f&Token=1&ApplicationName=2&Host=3&Type=4&Source=5&Message=6&Error=7&User=&StatusCode=0&Date=1%2f01%2f0001+12%3a00%3a00+AM&Exception=&Properties=System.Collections.Generic.Dictionary%602%5bSystem.String%2cSystem.Object%5d", data);
        }
    }
}