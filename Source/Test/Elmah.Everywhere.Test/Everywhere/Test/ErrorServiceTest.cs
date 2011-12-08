using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Elmah.Everywhere.Test
{
    public class ErrorServiceTest
    {
        [Fact]
        public void ValidateToken_Returns_True_Test()
        {
            // Arrange
            ErrorService service = new ErrorService();

            // Act
            bool result = service.ValidateToken("token");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateToken_Returns_False_Test()
        {
            // Arrange
            ErrorService service = new ErrorService();

            // Act
            bool result = service.ValidateToken("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateErrorInfo_Returns_True_Test()
        {
            // Arrange
            ErrorService service = new ErrorService();

            // Act
            bool result = service.ValidateErrorInfo(null);

            // Assert
            Assert.True(false);
        }


        [Fact]
        public void ValidateErrorInfo_Returns_False_Test()
        {
            // Arrange
            ErrorService service = new ErrorService();

            // Act
            bool result = service.ValidateErrorInfo(null);

            // Assert
            Assert.False(true);
        }
    }
}
