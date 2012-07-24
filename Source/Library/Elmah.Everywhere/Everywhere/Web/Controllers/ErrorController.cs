using System;
using System.Web.Mvc;
using System.Text;
using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers
{
    [CustomRequireHttps]
    public class ErrorController : Controller
    {
        private readonly IErrorService _errorService;
        private readonly ElmahErrorHelper _elmahErrorHelper;

        public ErrorController() : this(new ElmahErrorHelper(), new ErrorService())
        { 
        }

        public ErrorController(ElmahErrorHelper elmahErrorHelper, IErrorService errorService)
        {
            if (elmahErrorHelper == null)
            {
                throw new ArgumentNullException("elmahErrorHelper");
            }
            if (errorService == null)
            {
                throw new ArgumentNullException("errorService");
            }
            _errorService = errorService;
            _elmahErrorHelper = elmahErrorHelper;
        }

        [HttpPost]
        public ActionResult Log(string token, string error)
        {
            if (_errorService.ValidateToken(token))
            {
                var bytes = Convert.FromBase64String(error);
                var xml = Encoding.UTF8.GetString(bytes);
                var info = Utils.Utility.DeserializeXml(xml);
                if (_errorService.ValidateErrorInfo(info))
                {
                    _elmahErrorHelper.LogException(info);
                    return new HttpStatusCodeResult(200);
                }
                return new HttpStatusCodeResult(412);
            }
            return new HttpStatusCodeResult(403);
        }
    }
}
