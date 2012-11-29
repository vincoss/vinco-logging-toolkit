using System;
using Xunit;
using Moq;
using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere.ServiceModel.Test
{
    public class HttpErrorHandlerTest
    {
        [Fact]
        public void HandleError_Test()
        {
            // Arange
            HttpErrorHandler handler = new HttpErrorHandler();
            ExceptionDefaults defaults = new ExceptionDefaults
                                             {
                                                 ApplicationName = "ApplicationName",
                                                 Host = "Host",
                                                 Token = "Token"
                                             };

            Mock<ExceptionWritterBase> writterMock = new Mock<ExceptionWritterBase>();
            writterMock.Setup(x => x.Write(defaults.Token, It.IsAny<ErrorInfo>())).Verifiable();

            ExceptionHandler.Configure(writterMock.Object, defaults, null);

            // Act
            bool result = handler.HandleError(new Exception("Test-Exception"));

            // Assert
            Assert.False(result);
            writterMock.Verify();
        }
    }
}