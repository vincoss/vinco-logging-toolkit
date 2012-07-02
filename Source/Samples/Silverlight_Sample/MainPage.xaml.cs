using System.Windows;
using System.Windows.Controls;
using WcfRia_HostWebSite.Services;
using Elmah.Everywhere.Diagnostics;


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
            new SampleWcfRiaContext().GetMessage("World!!!", (operation) =>
                                                                 {
                                                                     string message = null;
                                                                     if (operation.Error != null)
                                                                     {
                                                                         message = operation.Error.ToString();
                                                                     }
                                                                     else
                                                                     {
                                                                         message = operation.Value;
                                                                     }
                                                                     tblMessage.Text = message;
                                                                 }, null);
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            new SampleWcfRiaContext().MakeError((operation)=>
                                                    {
                                                        string message = "All good!!!";
                                                        if (operation.Error != null)
                                                        {
                                                            message = operation.Error.ToString();
                                                        }
                                                        tblMessage.Text = message;
                                                    }, null);
           
        }

        private void btnClientError_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}