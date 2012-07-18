using Xunit;


namespace Elmah.Everywhere.ServiceModel.Test
{
    public class ErrorBehaviorExtensionElementTest
    {
        [Fact]
        public void BehaviorType_Test()
        {
            // Arrange
            ErrorBehaviorExtensionElement behavior = new ErrorBehaviorExtensionElement();

            // Assert
            Assert.True(behavior.BehaviorType.FullName == typeof(ServiceHttpErrorBehaviorAttribute).FullName);
        }
    }
}