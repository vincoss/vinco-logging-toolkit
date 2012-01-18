using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlight_Sample.Web
{
    public partial class MakeError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            throw new AmbiguousMatchException("Test Error...");
        }
    }
}