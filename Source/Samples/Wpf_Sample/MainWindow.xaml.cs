using System.Windows;
using Elmah.Everywhere.Diagnostics;


namespace Wpf_Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Make an exception

            int i = 0;
            int result = 10 / i;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var client = new WcfSample.SampleWcfServiceClient();
            client.MakeErrorCompleted += (s, args) =>
                                             {
                                                 if(args.Error != null)
                                                 {
                                                     // Manual error log
                                                     ExceptionHandler.Report(args.Error);

                                                     MessageBox.Show(args.Error.Message, "Error");
                                                 }
                                             };
            client.MakeErrorAsync();
        }
    }
}
