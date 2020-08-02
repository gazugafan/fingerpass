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
	public partial class SetMasterPasswordForm : Form
	{
		public SetMasterPasswordForm()
		{
			InitializeComponent();
		}

		private void SetMasterPasswordForm_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedDialog; //some DPI madness requires this to be set to sizable to start

			SetupForm();

			this.Activate();
		}

		private void SetupForm()
		{
			if (Properties.Settings.Default.MasterCheckHash == "")
			{
				currentPasswordLabel.Visible = false;
				currentPasswordTextbox.Visible = false;
				deleteLink.Visible = false;
				instructionsLabel.Text = "To start, please set your master password. You'll need to type this in occassionally to unlock your password database. Be sure to use a good one to keep things secure.";
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			//make sure the confirmation password matches...
			if (newPasswordTextbox.Text != confirmPasswordTextbox.Text)
			{
				MessageBox.Show("The confirmation password does not match the new password", "Hold up", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				//if no password was set before, destroy any existing database and set the new password...
				if (Properties.Settings.Default.MasterCheckHash == "")
				{
					Program.keyDatabase.UpdatePassword(newPasswordTextbox.Text);
				}
				else
				{
					//there was a password already set, so we need to update the database...
					Program.keyDatabase.UpdatePassword(newPasswordTextbox.Text, currentPasswordTextbox.Text);
				}

				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void deleteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (MessageBox.Show("THIS WILL DELETE ALL OF YOUR SAVED PASSWORDS. ONLY PROCEED IF YOU'VE FORGOTTEN YOUR MASTER PASSWORD. ARE YOU SURE YOU WANT TO DELETE YOUR DATABASE?", "DANGER!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
			{
				if (MessageBox.Show("One last chance to abort. Just triple-checking... you really want to reset your password database?", "DANGER!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					try
					{
						Program.keyDatabase.Destroy();
						SetupForm();
						MessageBox.Show("Password database reset! You can set a new password now.", "Okay!", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Something went wrong... " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}
	}
}
