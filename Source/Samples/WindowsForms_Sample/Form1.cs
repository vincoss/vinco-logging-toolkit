using Elmah.Everywhere.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsForms_Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMakeUIException_Click(object sender, EventArgs e)
        {
            ThrowException();
        }

        private async void btnAsyncUIException_Click(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(ThrowException);
        }

        private void ThrowException()
        {
            // Make an exception
            int i = 0;
            int result = 10 / i;
        }

        private async void btnServiceCallException_Click(object sender, EventArgs e)
        {
            var client = new WcfSample.SampleWcfServiceClient();
            await client.MakeErrorAsync();
        }
    }
}