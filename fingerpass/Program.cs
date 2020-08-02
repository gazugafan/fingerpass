using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;

namespace gazugafan.fingerpass.tray
{
	class Program
	{
		static Mutex mutex = new Mutex(false, "gazugafan.fingerpass");
		public static KeyDatabase keyDatabase;
		public static TrayIcon trayIcon = null;

		/// <summary>
		/// Application entry point.
		/// </summary>
		/// <param name="args">Command-line arguments.</param>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Wait 5 seconds if contended – in case another instance
			// of the program is in the process of shutting down.
			if (!mutex.WaitOne(TimeSpan.FromSeconds(5), false))
			{
				return;
			}

			try
			{
				//open the database...
				keyDatabase = new KeyDatabase();

				//show the tray icon...
				trayIcon = new TrayIcon();
				trayIcon.Show();

				Application.Run();
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}
	}
}
