using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Vinco.ElmahHandler.Diagnostics;


namespace Wpf_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create exception and sample data.
            Exception exception = GetSampleException();
            exception.Data.Add("Some-Key", "Some-Value");

            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Test", "Value 1");

            // Report exception
            ExceptionHandler.Report(exception, properties);
        }

        public static Exception GetSampleException()
        {
            Exception exception = null;
            try
            {
                int i = 0;
                int result = 10 / i;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }
    }
}
