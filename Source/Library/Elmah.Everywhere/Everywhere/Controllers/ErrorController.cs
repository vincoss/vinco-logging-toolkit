using System;
using System.Web.Mvc;


namespace Elmah.Everywhere.Controllers
{
    // TODO: Should allow only HTTPS
    public class ErrorController : Controller
    {
        private readonly IErrorService _errorService;
        private readonly ElmahErrorHelper _elmahErrorHelper;

        public ErrorController() : this(new ElmahErrorHelper(), new ErrorService())
        { 
        }

        public ErrorController(ElmahErrorHelper elmahErrorHelper, IErrorService errorService)
        {
            _errorService = errorService;
            _elmahErrorHelper = elmahErrorHelper;
        }

        [HttpPost]
        public ActionResult Log(ErrorInfo model)
        {
            if (_errorService.ValidateToken(model.Token))
            {
                if (_errorService.ValidateErrorInfo(model))
                {
                    _elmahErrorHelper.LogException(model);
                    return new HttpStatusCodeResult(200);
                }
                return new HttpStatusCodeResult(412);
            }
            return new HttpStatusCodeResult(403);
        }
    }
}
