using System;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web.Mvc;
using Xunit;
using Moq;

using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers.Test
{
    public class ElmahControllerTest
    {
        [Fact]
        public void Has_AuthorizeAttribute_Test()
        {
            // Act
            var attributes = typeof(ElmahController).GetCustomAttributes(typeof(AuthorizeAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }

        [Fact]
        public void Index_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Index(null) as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
