using Xunit;


namespace Elmah.Everywhere.Test
{
    public class WritterEventArgsTest
    {
        [Fact]
        public void Error_Should_Not_Be_Null_Test()
        {
            // Act
            WritterEventArgs args = new WritterEventArgs(new ErrorInfo());

            // Assert
            Assert.NotNull(args.Error);
        }
    }
}
