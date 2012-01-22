using System;
using System.ServiceModel.Configuration;


namespace Elmah.Everywhere.ServiceModel
{
    public class ErrorBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(ServiceHttpErrorBehaviorAttribute); }
        }

        protected override object CreateBehavior()
        {
            return new ServiceHttpErrorBehaviorAttribute(typeof(HttpErrorHandler));
        }
    }
}