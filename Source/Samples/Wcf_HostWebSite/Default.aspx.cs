﻿using System;


namespace Wcf_HostWebSite
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Wcf_HostWebSite.WcfSample.SampleWcfServiceClient _client = new Wcf_HostWebSite.WcfSample.SampleWcfServiceClient();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMessage_Click(object sender, EventArgs e)
        {
            litMessage.Text = _client.GetMessage("World!!!");
        }

        protected void btnError_Click(object sender, EventArgs e)
        {
            _client.MakeError();
        }
    }
}