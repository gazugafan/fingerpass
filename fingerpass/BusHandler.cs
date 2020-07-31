using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fastipc.Bus;
using gazugafan.fingerpass.messages;

namespace gazugafan.fingerpass.tray
{
	public class BusHandler : IHandleMessage
	{
		private readonly IBus _bus;
		private TrayIcon _trayIcon;

		public BusHandler(IBus bus, TrayIcon trayIcon)
		{
			_bus = bus;
			_trayIcon = trayIcon;
			_bus.Subscribe(this);
		}

		public void Handle(fastipc.Message.Message msg)
		{
			HandleInternal((dynamic)msg);
		}

		private void HandleInternal(ServiceAnnouncement msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Tray received a service announcement");

			//the service just started. get it's status (and trigger it to start listening)...
			_trayIcon.Publish(new GetStatus());
		}

		private void HandleInternal(Identified msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Tray received an identified message " + msg.UnitID.ToString());
			_trayIcon.Identified(msg.UnitID.ToString());
		}

		private void HandleInternal(UnknownIdentity msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Tray received an unknown identity message");
			_trayIcon.Unknown();
		}

		private void HandleInternal(StatusUpdate msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Tray received a service status update: " + msg.status.ToString());
			_trayIcon.UpdateStatus(msg.status);
		}
	}
}