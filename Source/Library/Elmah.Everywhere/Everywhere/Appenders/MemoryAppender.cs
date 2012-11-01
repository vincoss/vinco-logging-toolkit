using System.Runtime.InteropServices;
using System.Globalization;


namespace Elmah.Everywhere.Appenders
{
    public class MemoryAppender : BaseAppender
    {
        public override void Append(ErrorInfo error)
        {
            var memorystatusex = new MemoryStatusEx();
            if (NativeMethods.GlobalMemoryStatusEx(memorystatusex))
            {
                error.AddDetail(this.Name, "Total Memory", string.Format(CultureInfo.InvariantCulture, "{0} MB", memorystatusex.ullTotalPhys / (1024 * 1024)));
                error.AddDetail(this.Name, "Available Memory", string.Format(CultureInfo.InvariantCulture, "{0} MB", memorystatusex.ullAvailPhys / (1024 * 1024)));
            }
        }

        #region Nested types

        private static class NativeMethods
        {
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);    
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MemoryStatusEx
        {
            private readonly uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MemoryStatusEx()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));
            }
        }

        #endregion

        public override int Order
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "Memory Appender"; }
        }
    }
}
