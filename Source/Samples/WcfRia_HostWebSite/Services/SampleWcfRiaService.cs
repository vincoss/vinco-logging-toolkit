using System;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Hosting;


namespace WcfRia_HostWebSite.Services
{
    [EnableClientAccess]
    public class SampleWcfRiaService : DomainService
    {
        [Invoke]
        public string GetMessage(string key)
        {
            return "Hello " + key + " " + DateTime.Now;
        }

        [Invoke]
        public void MakeError()
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}