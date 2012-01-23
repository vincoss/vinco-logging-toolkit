using System.Runtime.InteropServices;
using System.Text;


namespace Elmah.Everywhere
{
    public class MemoryDetail : LogDetailBase
    {
        public override void Append(StringBuilder sb)
        {
            var memorystatusex = new MemoryStatusEx();
            if (GlobalMemoryStatusEx(memorystatusex))
            {
                sb.AppendLine(string.Format("Total Memory:              {0} MB", memorystatusex.ullTotalPhys / (1024 * 1024)));
                sb.AppendLine(string.Format("Available Memory:          {0} MB", memorystatusex.ullAvailPhys / (1024 * 1024)));
            }
        }

        #region Nested types

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);

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

        public override string Name
        {
            get { return "MemoryDetail"; }
        }
    }
}
