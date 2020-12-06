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
	public partial class MasterPasswordPromptForm : Form
	{
		private string _prompt = null;

		public MasterPasswordPromptForm(string prompt = null)
		{
			_prompt = prompt;
			InitializeComponent();
		}

		private void MasterPasswordPromptForm_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedDialog; //some DPI madness requires this to be set to sizable to start

			if (_prompt != null)
				instructionsLabel.Text = _prompt + " ";
			else
				instructionsLabel.Text = "";

			instructionsLabel.Text += "Please enter your master password to continue...";

			this.ShowInTaskbar = true;
			this.Activate();
			this.BringToFront();
			this.TopMost = true;
		}

		private void continueButton_Click(object sender, EventArgs e)
		{
			string password = passwordTextbox.Text;
			if (password.Trim() != "")
			{
				try
				{
					Program.keyDatabase.Unlock(password);
					this.Close();
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
				MessageBox.Show("Please enter your current master password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
