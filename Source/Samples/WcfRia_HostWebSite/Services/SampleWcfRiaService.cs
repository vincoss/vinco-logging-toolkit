using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;

using Elmah.Everywhere.ServiceModel;


namespace WcfRia_HostWebSite.Services
{
    [EnableClientAccess()]
    //[ServiceHttpErrorBehavior(typeof(HttpErrorHandler))]
    public class SampleWcfRiaService : DomainService
    {
        [Invoke]
        public string GetMessage(string key)
        {
            return "Hello " + key;
        }

        [Invoke]
        public void MakeError()
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}