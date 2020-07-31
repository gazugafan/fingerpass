using fastipc.Message;
using ProtoBuf;

namespace gazugafan.fingerpass.messages
{
	public enum Statuses
	{
		Starting,
		NoDevice,
		NoSession,
		NoFocus,
		Failing,
		Listening,
		CommunicationFailure,
		ServiceStopped, //service was elegantly stopped
		Paused,
	}

	[ProtoContract]
	public class GetStatus : Message { }

	[ProtoContract]
	public class UnknownIdentity : Message { }

	[ProtoContract]
	public class TrayClose : Message { }

	[ProtoContract]
	public class Pause : Message { }

	[ProtoContract]
	public class Resume : Message { }

	[ProtoContract]
	public class ServiceAnnouncement : Message { }

	[ProtoContract(SkipConstructor = true)]
	public class Identified : Message 
	{
		[ProtoMember(1)]
		public string UnitID { get; set; }

		public Identified(string unitID)
		{
			this.UnitID = unitID;
		}
	}

	[ProtoContract(SkipConstructor = true)]
	public class StatusUpdate : Message 
	{
		[ProtoMember(1)]
		public Statuses status { get; set; }

		public StatusUpdate(Statuses status)
		{
			this.status = status;
		}
	}
}