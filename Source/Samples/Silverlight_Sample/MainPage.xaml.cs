using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System;
using Elmah.Everywhere.Diagnostics;
using WcfRia_HostWebSite.Services;


namespace Silverlight_Sample
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        #region WCF

        private void wcfButtonMessage_Click(object sender, RoutedEventArgs e)
        {
            var client = new WcfSample.SampleWcfServiceClient();

            client.GetMessageCompleted += GetMessageCompleted;

            client.GetMessageAsync("WCF World!!!");
        }

        private void GetMessageCompleted(object sender, WcfSample.GetMessageCompletedEventArgs e)
        {
            string message = null;
            if (e.Error != null)
            {
                message = e.Error.ToString();
            }
            else
            {
                message = e.Result;
            }
            wcfTextBlockMessage.Text = message;
        }

        private void wcfButtonError_Click(object sender, RoutedEventArgs e)
        {
            var client = new WcfSample.SampleWcfServiceClient();

            client.MakeErrorCompleted += MakeErrorCompleted;

            client.MakeErrorAsync();
        }

        private void MakeErrorCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string message = "All good!!!";
            if (e.Error != null)
            {
                message = e.Error.ToString();
            }
            wcfTextBlockMessage.Text = message;
        }

        #endregion

        #region WCF RIA

        private void wcfRiaButtonMessage_Click(object sender, RoutedEventArgs e)
        {
            var client = new SampleWcfRiaContext(new Uri("http://localhost:62162/services/WcfRia_HostWebSite-Services-SampleWcfRiaService.svc"));

            var operation = client.GetMessage("WCF RIA world!!!");
            operation.Completed += GetMessageCompleted;
        }

        private void GetMessageCompleted(object sender, EventArgs e)
        {
            var oparation = (InvokeOperation)sender;

            string message = null;
            if (oparation.HasError && oparation.Error != null)
            {
                message = oparation.Error.ToString();
            }
            else
            {
                message = oparation.Value == null ? "" : oparation.Value.ToString();
            }
            wcfRiaTextBlockMessage.Text = message;
        }

        private void wcfRiaButtonClientError_Click(object sender, RoutedEventArgs e)
        {
            var client = new SampleWcfRiaContext(new Uri("http://localhost:62162/services/WcfRia_HostWebSite-Services-SampleWcfRiaService.svc"));

            var operation = client.MakeError();
            operation.Completed += MakeErrorCompleted;
        }

        private void MakeErrorCompleted(object sender, EventArgs e)
        {
            var oparation = (InvokeOperation)sender;

            string message = "All good!!!";
            if (oparation.HasError && oparation.Error != null)
            {
                message = oparation.Error.ToString();
            }
            wcfRiaTextBlockMessage.Text = message;
        }

        #endregion

        private void buttonClientError_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = 0;
                int result = 10 / i;
            }
            catch (Exception ex)
            {
                TextBlock_ClientMessage.Text = "10 / 0";
                ExceptionHandler.Report(ex, null);
            }
        } 

    }
}