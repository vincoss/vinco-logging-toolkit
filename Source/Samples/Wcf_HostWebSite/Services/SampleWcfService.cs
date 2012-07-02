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

    public class SampleWcfService : ISampleWcfService
    {
        public string GetMessage(string key)
        {
            return "Hello " + key;
        }

        public void MakeError()
        {
            int i = 0;
            int result = 10 / i;
        }
    }
}