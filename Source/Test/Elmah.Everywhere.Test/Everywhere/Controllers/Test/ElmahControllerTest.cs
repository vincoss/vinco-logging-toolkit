using System.Linq;
using System.Web.Mvc;
using Xunit;
using Elmah.Everywhere.Web;
using System;


namespace Elmah.Everywhere.Controllers.Test
{
    public class ElmahControllerTest
    {
        [Fact]
        public void Has_RequireHttpsAttribute_Test()
        {
            // Act
            var attributes = typeof(ElmahController).GetCustomAttributes(typeof(CustomRequireHttpsAttribute), false);

            // Assert
            Assert.True(attributes.Any());
        }

        [Fact]
        public void Has_AuthorizeAttribute_Test()
        {
            // Act
            var attributes = typeof(ElmahController).GetCustomAttributes(typeof(AuthorizeAttribute), false);

            // Assert
            Assert.True(attributes.Any());
            Assert.True(attributes.Cast<AuthorizeAttribute>().Any(x => string.Equals("Administrator", x.Roles, StringComparison.OrdinalIgnoreCase)));
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

        [Fact]
        public void Detail_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Detail() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Stylesheet_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Stylesheet() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Xml_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Xml() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DigestRss_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.DigestRss() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Download_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Download() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Rss_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Rss() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Json_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.Json() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void About_Returns_ElmahResult_Test()
        {
            // Arrange
            ElmahController controller = new ElmahController();

            // Act
            ElmahResult result = controller.About() as ElmahResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
