using System;


namespace Wcf_HostWebSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMessage_Click(object sender, EventArgs e)
        {
           // litMessage.Text = _client.GetMessage("World!!!");
        }

        protected void btnError_Click(object sender, EventArgs e)
        {
            //_client.MakeError();
        }
    }
}