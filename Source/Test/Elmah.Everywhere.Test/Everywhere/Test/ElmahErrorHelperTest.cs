using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace Elmah.Everywhere.Test
{
    public class ElmahErrorHelperTest
    {
        [Fact]
        public void LogException_ErrorInfo_Test()
        {
            // Arrange
            TestableElmahErrorHelper helper = new TestableElmahErrorHelper();

            ErrorInfo info = new ErrorInfo();

            // TODO: add error details

            // Act
            helper.LogException(info);

            // Assert
            Assert.NotNull(helper.Error);

            // TODO: Check error details
        }

        class TestableElmahErrorHelper : ElmahErrorHelper
        {
            public Error Error;

            protected override void LogInternal(Error error)
            {
                Error = error;
            }
        }
    }
}
