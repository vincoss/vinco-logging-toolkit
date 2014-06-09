using Microsoft.Phone.Net.NetworkInformation;
using System.Collections.Generic;


namespace Elmah.Everywhere.Appenders
{
    public class DeviceNetworkAppender : BaseAppender
    {
        public override void Append(ErrorInfo errorInfo)
        {
            var pairs = new Dictionary<string, string>();
            pairs.Add("CellularMobileOperator", DeviceNetworkInformation.CellularMobileOperator);
            pairs.Add("IsCellularDataEnabled", DeviceNetworkInformation.IsCellularDataEnabled.ToString());
            pairs.Add("IsCellularDataRoamingEnabled", DeviceNetworkInformation.IsCellularDataRoamingEnabled.ToString());
            pairs.Add("IsNetworkAvailable", DeviceNetworkInformation.IsNetworkAvailable.ToString());
            pairs.Add("IsWiFiEnabled", DeviceNetworkInformation.IsWiFiEnabled.ToString());

            errorInfo.AddDetail(this.Name, pairs);
        }

        public override string Name
        {
            get { return "Device Network Appender"; }
        }
    }
}