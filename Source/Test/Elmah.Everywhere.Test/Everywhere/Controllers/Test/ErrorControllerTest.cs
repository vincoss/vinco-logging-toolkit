using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Moq;
using Xunit;
using Elmah.Everywhere.Utils;
using System.Text;
using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers.Test
{
    public class ErrorControllerTest
    {
        [Fact]
        public void Has_RequireHttpsAttribute_Test()
        {
            // Act
            var attributes = typeof(ErrorController).GetCustomAttributes(typeof(CustomRequireHttpsAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }

        [Fact]
        public void Log_Has_HttpPostAttribute_Test()
        {
            // Act
            // Arrange
            Expression<Func<ErrorController, ActionResult>> expression = x => x.Log(null, null);
            var methodCallExpression = (MethodCallExpression)expression.Body;

            // Act
            var attributes = methodCallExpression.Method.GetCustomAttributes(typeof(HttpPostAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }

        [Fact]
        public void Log_Returns_HttpStatusCodeResult_200_Test()
        {
            // Arrange
            TestableElmahErrorHelper helper = new TestableElmahErrorHelper();
            Mock<IErrorService> service = new Mock<IErrorService>();

            service.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(true);
            service.Setup(x => x.ValidateErrorInfo(It.IsAny<ErrorInfo>())).Returns(true);

            ErrorController controller = new ErrorController(helper, service.Object);

            ErrorInfo info = new ErrorInfo(new Exception("Test-Exception"));

            string xml = Utility.SerializeXml(info);
            string error = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));

            // Act
            HttpStatusCodeResult result = controller.Log("Test-Token", error) as HttpStatusCodeResult;

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(helper.Error);
        }

        [Fact]
        public void Put_Returns_HttpStatusCodeResult_403_Test()
        {
            // Arrange
            Mock<IErrorService> service = new Mock<IErrorService>();

            service.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(false);

            ErrorController controller = new ErrorController(new TestableElmahErrorHelper(), service.Object);

            // Act
            HttpStatusCodeResult result = controller.Log("", null) as HttpStatusCodeResult;

            // Assert
            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public void Put_Returns_HttpStatusCodeResult_412_Test()
        {
            // Arrange
            Mock<IErrorService> service = new Mock<IErrorService>();

            service.Setup(x => x.ValidateToken(It.IsAny<string>())).Returns(true);

            ErrorController controller = new ErrorController(new TestableElmahErrorHelper(), service.Object);

            ErrorInfo info = new ErrorInfo(new Exception("Test-Exception"));

            string xml = Utility.SerializeXml(info);
            string error = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));

            // Act
            HttpStatusCodeResult result = controller.Log("Test-Token", error) as HttpStatusCodeResult;

            // Assert
            Assert.Equal(412, result.StatusCode);
        }

        class TestableElmahErrorHelper : ElmahErrorHelper
        {
            public Error Error;

            protected override void LogInternal(Error error, object context)
            {
                Error = error;
            }
        }
    }
}
