using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace fingerpass_service
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		private EventLogInstaller eventLogInstaller;

		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
