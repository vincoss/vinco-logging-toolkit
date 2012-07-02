using System.Linq;
using System.Web.Mvc;
using Xunit;
using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers.Test
{
    // TODO: complete remaining actions
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
            ElmahResult result = controller.Index() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
