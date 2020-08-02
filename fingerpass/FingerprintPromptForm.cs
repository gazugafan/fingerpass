using gazugafan.fingerpass.messages;
using gazugafan.fingerpass.Properties;
using gazugafan.fingerpass.tray;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gazugafan.fingerpass
{
	public partial class FingerprintPromptForm : Form
	{
		private string _prompt = null;

		public FingerprintPromptForm(string prompt = null)
		{
			_prompt = prompt;
			InitializeComponent();
		}

		private void FingerprintPromptForm_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedDialog; //some DPI madness requires this to be set to sizable to start

			//if the database isn't already unlocked, we just need to prompt for the master password instead...
			if (Program.keyDatabase.IsUnlocked())
			{
				Program.trayIcon.onIdentified = Identified; //capture fingerprint scans for this form
				iconPictureBox.Image = Bitmap.FromHicon(new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons/grey.ico"), new Size(64, 64)).Handle);

				if (_prompt != null)
					instructionsLabel.Text = _prompt + " ";
				else
					instructionsLabel.Text = "";

				instructionsLabel.Text += "Please scan your fingerprint to continue...";

				this.Activate();
			}
			else
			{
				promptForPasswordInstead();
				Close();
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void passwordButton_Click(object sender, EventArgs e)
		{
			promptForPasswordInstead();
		}

		public void promptForPasswordInstead()
		{
			Program.keyDatabase.EndSession();
			new MasterPasswordPromptForm(_prompt).ShowDialog();
			if (Program.keyDatabase.IsUnlocked())
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		public void Identified()
		{
			//close on next tick...
			BeginInvoke(new MethodInvoker(delegate
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}));
		}

		private void FingerprintPromptForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.trayIcon.onIdentified = null; //allow fingerprint scans to function normally again
		}
	}
}
