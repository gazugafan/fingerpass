using gazugafan.fingerpass.Properties;
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
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			Dictionary<string, string> soundOptions = GetSoundOptions();
			SoundSelect.Items.AddRange(soundOptions.Values.ToArray());

			if (soundOptions.ContainsKey(Properties.Settings.Default.SoundPath)) SoundSelect.Text = soundOptions[Properties.Settings.Default.SoundPath];
			else SoundSelect.Text = Properties.Settings.Default.SoundPath;

			timeoutNumeric.Value = Properties.Settings.Default.SessionMinutes;

			//get the windows startup value...
			RegistryKey startupKeys = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			startupCheckbox.Checked = (startupKeys.GetValue(Application.ProductName, "").ToString() != "");
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			//set the windows startup...
			RegistryKey startupKeys = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			if (startupCheckbox.Checked)
				startupKeys.SetValue(Application.ProductName, Application.ExecutablePath);
			else
				startupKeys.DeleteValue(Application.ProductName, false);

			Properties.Settings.Default.SessionMinutes = (int)timeoutNumeric.Value;

			Properties.Settings.Default.Save();

			this.Close();
		}

		private void SoundSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			Dictionary<string, string> soundOptions = GetSoundOptions();

			if (SoundSelect.Text == "Custom...")
			{
				OpenFileDialog file = new OpenFileDialog();
				file.Filter = "WAV Files|*.wav";
				if (file.ShowDialog() == DialogResult.OK)
				{
					SoundSelect.Text = file.FileName;
				}
			}

			if (soundOptions.ContainsValue(SoundSelect.Text)) Properties.Settings.Default.SoundPath = soundOptions.FirstOrDefault(x => x.Value == SoundSelect.Text).Key;
			else Properties.Settings.Default.SoundPath = SoundSelect.Text;
		}

		private Dictionary<string, string> GetSoundOptions()
		{
			Dictionary<string, string> results = new Dictionary<string, string>();
			results.Add("", "None");
			string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Sounds", "*.wav", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToArray();
			foreach (string file in files)
			{
				results.Add("{0}" + file, Path.GetFileNameWithoutExtension(file));
			}

			results.Add("Custom", "Custom...");

			return results;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.SoundPath != "")
			{
				System.Media.SoundPlayer player = new System.Media.SoundPlayer(String.Format(Properties.Settings.Default.SoundPath, AppDomain.CurrentDomain.BaseDirectory + "Sounds\\"));
				player.Play();
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Reload();
			this.Close();
		}

		private void managePasswordsButton_Click(object sender, EventArgs e)
		{
			new DatabaseForm().ShowDialog();
		}
	}
}
