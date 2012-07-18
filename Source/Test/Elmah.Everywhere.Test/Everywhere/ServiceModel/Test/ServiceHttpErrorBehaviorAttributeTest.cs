using System;
using System.Linq;
using Xunit;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Moq;
using System.ServiceModel.Channels;


namespace Elmah.Everywhere.ServiceModel.Test
{
    public class ServiceHttpErrorBehaviorAttributeTest
    {
        [Fact]
        public void Constructor_Throws_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         new ServiceHttpErrorBehaviorAttribute(null);
                                                     });
        }

        [Fact]
        public void ApplyDispatchBehavior_Test()
        {
            // Arrange
            ServiceHostBase host = new TestableServiceHostBase();
            Mock<IChannelListener> listener = new Mock<IChannelListener>();

            ChannelDispatcher dispatcher = new ChannelDispatcher(listener.Object);
            host.ChannelDispatchers.Add(dispatcher);

            ServiceHttpErrorBehaviorAttribute attribute = new ServiceHttpErrorBehaviorAttribute(typeof(HttpErrorHandler));

            // Act
            attribute.ApplyDispatchBehavior(null, host);

            // Assert
            Assert.IsType<HttpErrorHandler>(dispatcher.ErrorHandlers.First());
        }

        private class TestableServiceHostBase : ServiceHostBase
        {
            public TestableServiceHostBase()
            {
            }

            protected override System.ServiceModel.Description.ServiceDescription CreateDescription(out System.Collections.Generic.IDictionary<string, System.ServiceModel.Description.ContractDescription> implementedContracts)
            {
                throw new NotImplementedException();
            }
        }
    }
}
