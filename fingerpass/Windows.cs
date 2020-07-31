using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace gazugafan.fingerpass
{
	class Windows
	{

		[DllImport("user32.dll")]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Returns the name of the program of the currently focused window.
        /// </summary>
        /// <returns></returns>
		public static string GetFocusedApplicationName()
		{
            IntPtr handle = GetForegroundWindow();
            string fileName = "";
            string name = "";
            uint pid = 0;
            GetWindowThreadProcessId(handle, out pid);

            Process p = Process.GetProcessById((int)pid);
            var processname = p.ProcessName;

            switch (processname)
            {
                case "explorer": //metro processes
                case "WWAHost":
                    return processname;
                default:
                    break;
            }

            string wmiQuery = string.Format("SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId LIKE '{0}'", pid.ToString());
            var pro = new ManagementObjectSearcher(wmiQuery).Get().Cast<ManagementObject>().FirstOrDefault();
            fileName = (string)pro["ExecutablePath"];
            // Get the file version
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            // Get the file description
            name = myFileVersionInfo.FileDescription;
            if (name == "")
                return myFileVersionInfo.FileName;

            return name;
        }

        /// <summary>
        /// Returns the window title of the currently focused window
        /// </summary>
        /// <returns></returns>
		public static string GetFocusedWindowTitle()
		{
            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            // Obtain the length of the text   
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0)
            {
                strTitle = stringBuilder.ToString();
            }
            return strTitle;
        }
    }
}
