using Elmah.Everywhere.Utils;
using Microsoft.Devices;
using Microsoft.Phone.Info;
using System.Collections.Generic;


namespace Elmah.Everywhere.Appenders
{
    public class DeviceStatusAppender : BaseAppender
    {
        public override void Append(ErrorInfo errorInfo)
        {
            var pairs = new Dictionary<string, string>();
            pairs.Add("ApplicationCurrentMemoryUsage", Utility.FormatBytes(DeviceStatus.ApplicationCurrentMemoryUsage));
            pairs.Add("ApplicationMemoryUsageLimit", Utility.FormatBytes(DeviceStatus.ApplicationMemoryUsageLimit));
            pairs.Add("ApplicationPeakMemoryUsage", Utility.FormatBytes(DeviceStatus.ApplicationPeakMemoryUsage));
            pairs.Add("DeviceFirmwareVersion", DeviceStatus.DeviceFirmwareVersion);
            pairs.Add("DeviceHardwareVersion", DeviceStatus.DeviceHardwareVersion);
            pairs.Add("DeviceManufacturer", DeviceStatus.DeviceManufacturer);
            pairs.Add("DeviceName", DeviceStatus.DeviceName);
            pairs.Add("DeviceTotalMemory", Utility.FormatBytes(DeviceStatus.DeviceTotalMemory));
            pairs.Add("IsKeyboardDeployed", DeviceStatus.IsKeyboardDeployed.ToString());
            pairs.Add("IsKeyboardPresent", DeviceStatus.IsKeyboardPresent.ToString());
            pairs.Add("PowerSource", DeviceStatus.PowerSource.ToString());
            pairs.Add("DeviceType", Environment.DeviceType.ToString());

            errorInfo.AddDetail(this.Name, pairs);
        }

        public override int Order
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "Device Status Appender"; }
        }
    }
}
