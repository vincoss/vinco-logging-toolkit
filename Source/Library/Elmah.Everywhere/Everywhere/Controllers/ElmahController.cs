using System;
using System.Web.Mvc;
using System.Dynamic;


namespace Elmah.Everywhere.Controllers
{
    // TODO: 
    // Validate mandatory properties
    // Encrypt and encode data

    public class HomeController : Controller
    {
        private readonly ElmahErrorHelper _elmahErrorHelper;

        public HomeController() : this(new ElmahErrorHelper())
        { 
        }

        public HomeController(ElmahErrorHelper elmahErrorHelper)
        {
            _elmahErrorHelper = elmahErrorHelper;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }

        [HttpPost]
        public ActionResult Put(string token, string applicationName, string host, string type, string source, string message, string error, DateTime? date)
        {
            if (TokenValid(token))
            {
                dynamic errorInfo = new ExpandoObject();
                errorInfo.ApplicationName = applicationName;
                errorInfo.HostName = host;
                errorInfo.Type = type;
                errorInfo.Source = source;
                errorInfo.Message = message;
                errorInfo.Detail = error;
                errorInfo.Time = date ?? DateTime.Now;

                _elmahErrorHelper.LogException(errorInfo, null);

                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(403);
        }

        [NonAction]
        private bool TokenValid(string token)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            // TODO: Validate token here
            // Possible host

            return true;
        }
    }
}