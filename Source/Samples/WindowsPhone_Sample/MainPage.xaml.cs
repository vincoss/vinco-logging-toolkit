using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone_8_Sample.Resources;
using Elmah.Everywhere.Diagnostics;


namespace WindowsPhone_8_Sample
{
    public partial class MainPage : PhoneApplicationPage
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

                ExceptionHandler.Report(e.Error);
            }
            wcfTextBlockMessage.Text = message;
        }

        #endregion

        private void ButtonClientError_Click(object sender, RoutedEventArgs e)
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