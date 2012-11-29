using System;
using System.Web.Mvc;


namespace Elmah.Everywhere.Web
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (IsEnabled())
            {
                base.OnAuthorization(filterContext);
            }
        }

        protected virtual bool IsEnabled()
        {
            return System.Web.Security.Roles.Enabled;
        }
    }
}
