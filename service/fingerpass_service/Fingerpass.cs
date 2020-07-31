using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using fastipc.Bus;
using fastipc.Message;
using gazugafan.fingerpass.messages;

namespace gazugafan.fingerpass.service
{
	public partial class Fingerpass : ServiceBase
	{
		public static FingerprintScanner FingerprintScanner;
		public NamedPipeBus bus = null;
		private BusHandler _busHandler;

		public Fingerpass()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			//EventLog.WriteEntry("starting...", EventLogEntryType.Information);

			RecreateBusConnection();

			//let the tray icon know the service just started. It should then let the service know to start listening...
			Publish(new ServiceAnnouncement());

			FingerprintScanner = new FingerprintScanner();
			FingerprintScanner._service = this;
			FingerprintScanner.onIdentify = this.OnIdentify;
			FingerprintScanner.onUnknownIdentity = this.OnUnknownIdentity;
			FingerprintScanner.Process();
		}

		protected override void OnStop()
		{
			//EventLog.WriteEntry("stopping...");
			FingerprintScanner.Stop();

			if (bus != null)
				bus.Dispose();
		}

		protected void OnUnknownIdentity()
		{
			//EventLog.WriteEntry("unknown identity");
			Publish(new UnknownIdentity());
		}

		protected void OnIdentify(string identity)
		{
			//EventLog.WriteEntry("identified " + identity);
			Publish(new Identified(identity));
		}

		/// <summary>
		/// The tray icon has requested a status update.
		/// This also means the tray icon must be connected now, so if we're not already listening, restart
		/// </summary>
		public void GetStatus()
		{
			if (FingerprintScanner.status != Statuses.Listening) 
				FingerprintScanner.RestartBiometricSession();
			else
				FingerprintScanner.SendStatus();
		}

		/// <summary>
		/// Releases focus and pauses listening.
		/// </summary>
		public void TrayClose()
		{
			FingerprintScanner.Pause();
			RecreateBusConnection();
		}

		/// <summary>
		/// Releases focus and pauses listening.
		/// </summary>
		public void Pause()
		{
			FingerprintScanner.Pause();
		}

		/// <summary>
		/// Releases focus and pauses listening.
		/// </summary>
		public void Resume()
		{
			FingerprintScanner.Resume();
		}

		public void RecreateBusConnection()
		{
			//EventLog.WriteEntry("RecreateBusConnection");

			if (bus != null)
			{
				//EventLog.WriteEntry("disposing existing bus");
				bus.Dispose();
			}

			//open bus with tray...
			var pipeName = new SimpleStringPipeName(
				name: "gazugafanFingerpass",
				side: Side.In
			);
			bus = new NamedPipeBus(pipeName: pipeName);
			_busHandler = new BusHandler(bus, this);
		}





		/// <summary>
		/// Publishes a message on the bus, but on its own thread, with a 2 second timeout that will cancel the message
		/// </summary>
		public void Publish<T>(T msg) where T : Message
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				Exception ex = null;
				CancellationTokenSource cts = new CancellationTokenSource();
				Task task = Task.Run(() =>
				{
					try
					{
						using (cts.Token.Register(Thread.CurrentThread.Abort))
						{
							bus.Publish(msg);
						}
					}
					catch (Exception e)
					{
						if (!(e is ThreadAbortException))
							ex = e;
					}
				}, cts.Token);
				bool done = task.Wait(2000);
				if (ex != null)
					throw ex;
				if (!done)
					cts.Cancel();
			});
		}
	}
}
