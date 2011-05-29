using System;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace Vinco.ElmahHandler.ServiceModel
{
    public class ServiceHttpErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        private Type _errorHandlerType;

        public ServiceHttpErrorBehaviorAttribute(Type errorHandlerType)
        {
            this._errorHandlerType = errorHandlerType;
        }

        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;
            errorHandler = (IErrorHandler)Activator.CreateInstance(_errorHandlerType);

            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                cd.ErrorHandlers.Add(errorHandler);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        #endregion
    }

}
