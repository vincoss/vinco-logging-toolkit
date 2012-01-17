using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Xunit;
using System.Linq.Expressions;
using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers.Test
{
    public class AccountControllerTest
    {
        [Fact]
        public void Has_RequireHttpsAttribute_Test()
        {
            // Act
            var attributes = typeof(AccountController).GetCustomAttributes(typeof(CustomRequireHttpsAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }

        [Fact]
        public void Put_Has_HttpPostAttribute_Test()
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
    }
}
