using System;
using System.ServiceModel;


namespace Wcf_HostWebSite.Services
{
    [ServiceContract()]
    public interface ISampleWcfService
    {
        [OperationContract]
        string GetMessage(string key);

        [OperationContract]
        void MakeError();
    }

    //[ServiceHttpErrorBehavior(typeof(HttpErrorHandler))]
    public class SampleWcfService : ISampleWcfService
    {
        public string GetMessage(string key)
        {
            return "Hello " + key + " " + DateTime.Now;
        }

        public void MakeError()
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}