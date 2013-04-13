using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;
using System.Linq.Expressions;
using Elmah.Everywhere.Web;
using Elmah.Everywhere.Models;
using Moq;
using System.Web;
using System.Web.Routing;


namespace Elmah.Everywhere.Controllers.Test
{
    public class AccountControllerTest
    {
        //[Fact]
        //public void Has_RequireHttpsAttribute_Test()
        //{
        //    // Act
        //    var attributes = typeof(AccountController).GetCustomAttributes(typeof(CustomRequireHttpsAttribute), false);

        //    // Assert
        //    Assert.True(attributes.Any());
        //}

        [Fact]
        public void LogOn_Has_HttpPostAttribute_Test()
        {
            // Act
            // Arrange
            Expression<Func<AccountController, ActionResult>> expression = x => x.LogOn(null, null);
            var methodCallExpression = (MethodCallExpression)expression.Body;

            // Act
            var attributes = methodCallExpression.Method.GetCustomAttributes(typeof(HttpPostAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }
        
        [Fact]
        public void LogOn_Test()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            ActionResult result = controller.LogOn();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LogOn_Post_Test()
        {
            // Arrange
            TestableAccountController controller = new TestableAccountController();
            controller.UserValid = true;

            LogOnModel model = new LogOnModel();
            model.UserName = "UserName";
            model.Password = "Password";

            // Act
            RedirectToRouteResult result = controller.LogOn(model, "") as RedirectToRouteResult;

            // Assert
            Assert.True(controller.ModelState.IsValid);
            Assert.True(controller.ValidateUserCalled);
            Assert.True(controller.SetAuthCookieCalled);
            Assert.True(result.RouteValues.Any(x => x.Key == "action" && (string)x.Value == ""));
            Assert.True(result.RouteValues.Any(x => x.Key == "controller" && (string)x.Value == "Elmah"));
        }

        [Fact]
        public void LogOn_Post_With_Return_Url_Test()
        {
            // Arrange
            TestableAccountController controller = new TestableAccountController();
            controller.Url = GetUrlHelperForIsLocalUrl();
            controller.UserValid = true;

            LogOnModel model = new LogOnModel();
            model.UserName = "UserName";
            model.Password = "Password";

            // Act
            RedirectResult result = controller.LogOn(model, "/elmah") as RedirectResult;

            // Assert
            Assert.True(controller.ModelState.IsValid);
            Assert.True(controller.ValidateUserCalled);
            Assert.True(controller.SetAuthCookieCalled);
            Assert.Equal("/elmah", result.Url);
        }

        [Fact]
        public void LogOn_Post_Has_Validation_Errors_Test()
        {
            // Arrange
            TestableAccountController controller = new TestableAccountController();

            // Act
            RedirectToRouteResult result = controller.LogOn(new LogOnModel(), "") as RedirectToRouteResult;

            // Assert
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal("The user name or password provided is incorrect.", controller.ModelState[""].Errors.First().ErrorMessage);
        }

        [Fact]
        public void LogOff_Test()
        {
            // Arrange
            TestableAccountController controller = new TestableAccountController();

            // Act
            RedirectToRouteResult result = controller.LogOff() as RedirectToRouteResult;

            // Assert
            Assert.True(result.RouteValues.Any(x => x.Key == "action" && (string) x.Value == ""));
            Assert.True(result.RouteValues.Any(x => x.Key == "controller" && (string)x.Value == "Elmah"));
            Assert.True(controller.SignOutInternalCalled);
        }

        private static UrlHelper GetUrlHelperForIsLocalUrl()
        {
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(context => context.Request.Url).Returns(new Uri("http://www.faulthub.org/"));
            RequestContext requestContext = new RequestContext(contextMock.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext);
            return helper;
        }

        class TestableAccountController : AccountController
        {
            public bool SignOutInternalCalled;
            public bool ValidateUserCalled;
            public bool SetAuthCookieCalled;
            public bool UserValid;

            protected override void SignOutInternal()
            {
                SignOutInternalCalled = true;
            }

            protected override bool ValidateUser(string userName, string passowrd)
            {
                ValidateUserCalled = true;
                return UserValid;
            }

            protected override void SetAuthCookie(string userName, bool remember)
            {
                SetAuthCookieCalled = true;
            }
        }
    }
}
