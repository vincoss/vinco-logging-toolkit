using System;
using WcfRia_HostWebSite.Services;


namespace Wcf_HostWebSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMessage_Click(object sender, EventArgs e)
        {
            var service = new SampleWcfRiaService();
            litMessage.Text = service.GetMessage("World!!!");
        }

        protected void btnError_Click(object sender, EventArgs e)
        {
            var service = new SampleWcfRiaService();
            service.MakeError();
        }
    }
}