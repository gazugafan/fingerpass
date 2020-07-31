using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastipc.Bus;
using gazugafan.fingerpass.messages;

namespace gazugafan.fingerpass.service
{
	public class BusHandler : IHandleMessage
	{
		private readonly IBus _bus;
		private Fingerpass _fingerpass;

		public BusHandler(IBus bus, Fingerpass fingerpass)
		{
			_bus = bus;
			_fingerpass = fingerpass;
			_bus.Subscribe(this);
		}

		public void Handle(fastipc.Message.Message msg)
		{
			HandleInternal((dynamic)msg);
		}

		private void HandleInternal(GetStatus msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Service received a get status request");
			_fingerpass.GetStatus();
		}

		private void HandleInternal(TrayClose msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Service received a tray close message");
			_fingerpass.TrayClose();
		}

		private void HandleInternal(Pause msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Service received a pause message");
			_fingerpass.Pause();
		}

		private void HandleInternal(Resume msg)
		{
			//EventLog.WriteEntry("Fingerpass", "Service received a resume message");
			_fingerpass.Resume();
		}
	}
}