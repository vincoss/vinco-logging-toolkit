using System;
using System.ServiceModel.Configuration;


namespace Elmah.Everywhere.ServiceModel
{
    public class ErrorBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ServiceHttpErrorBehaviorAttribute(typeof(HttpErrorHandler));
        }

        public override Type BehaviorType
        {
            get { return typeof(ServiceHttpErrorBehaviorAttribute); }
        }
    }
}