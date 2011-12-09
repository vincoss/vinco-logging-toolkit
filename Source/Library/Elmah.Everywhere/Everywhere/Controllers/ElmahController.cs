using System;
using System.Web.Mvc;
using Elmah.Everywhere.Web;


namespace Elmah.Everywhere.Controllers
{
    // TODO: Uncomment in production, and also add allowed role
    //[Authorize(Roles = "Administrator")]
    public class ElmahController : Controller
    {
        public ActionResult Index()
        {
            return new ElmahResult(null);
        }

        public ActionResult Detail()
        {
            return new ElmahResult("detail");
        }

        public ActionResult Stylesheet()
        {
            return new ElmahResult("stylesheet");
        }

        public ActionResult Xml()
        {
            return new ElmahResult("xml");
        }

        public ActionResult DigestRss()
        {
            return new ElmahResult("digestrss");
        }

        public ActionResult Download()
        {
            return new ElmahResult("download");
        }

        public ActionResult Rss()
        {
            return new ElmahResult("rss");
        }

        public ActionResult Json()
        {
            return new ElmahResult("json");
        }

        public ActionResult About()
        {
            return new ElmahResult("about");
        }
    }
}