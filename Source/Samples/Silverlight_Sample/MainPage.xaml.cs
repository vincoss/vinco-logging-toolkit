using System.Windows;
using System.Windows.Controls;


namespace Silverlight_Sample
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnMessage_Click(object sender, RoutedEventArgs e)
        {
            var client = new WcfSample.SampleWcfServiceClient();

            client.GetMessageCompleted += GetMessageCompleted;

            client.GetMessageAsync("World!!!");
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
            tblMessage.Text = message;
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
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
            tblMessage.Text = message;
        }

        private void btnClientError_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}