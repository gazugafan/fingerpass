using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Timers;
using Microsoft.Win32;
using gazugafan.fingerpass.messages;
using System.Timers;
using System.Linq;
using fastipc.Bus;

namespace gazugafan.fingerpass.tray
{
	/// <summary>
	/// The main program
	/// </summary>
	public class TrayIcon : IDisposable
	{
		#region Init
		private Statuses _status;
		private NotifyIcon _notifyIcon;
		public NamedPipeBus bus;
		public bool isLightMode = true;
		private BusHandler _busHandler;
		private bool _isDisposed;
		private Icon _iconNormal;
		private Icon _iconGrey;
		private Icon _iconRed;
		private Icon _iconGreen;
		private System.Timers.Timer _flashTimer;
		private Icon _iconFlash;
		private string _recentProgramName;
		private string _recentWindowTitle;
		private KeyDatabase _database;

		public delegate void IdentifiedDelegate();
		public IdentifiedDelegate onIdentified = null;

		public delegate void ToastClickDelegate();
		public ToastClickDelegate onToastClick = null;

		/// <summary>
		/// Initialises a new instance of this class.
		/// </summary>
		public TrayIcon()
		{
			//maybe reset settings when debugging...
#if DEBUG
			//Properties.Settings.Default.Reset();
#endif

			this._status = Statuses.Listening;

			//setup hourglass animation icons...
			IsDarkMode();
			_iconNormal = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons/" + (isLightMode?"black.ico":"white.ico")));
			_iconGrey = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons/grey.ico"));
			_iconRed = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons/red.ico"));
			_iconGreen = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons/green.ico"));

			//create bus with service...
			var pipeName = new SimpleStringPipeName(
				name: "gazugafanFingerpass",
				side: Side.Out
			);
			bus = new NamedPipeBus(pipeName: pipeName);
			_busHandler = new BusHandler(bus, this);

			//create the tray icon...
			_notifyIcon = CreateNotifyIcon();
			UpdateStatusIcon();
			UpdateTrayMenu();

			//setup the flash timer...
			_flashTimer = new System.Timers.Timer(1000);
			_flashTimer.AutoReset = false;
			_flashTimer.Stop();
			_flashTimer.Elapsed += OnFlashTimer;

			//get the status from the service (also announcing that the tray icon is now available)...
			Publish(new GetStatus());
		}

		/// <summary>
		/// Class destruction and cleanup.
		/// </summary>
		~TrayIcon()
		{
			Dispose(false);
		}
		#endregion Init ----------------------------------------------------------------------------------------------

		/// <summary>
		/// Show the tray icon.
		/// </summary>
		public void Show()
		{
			if (_isDisposed) throw new ObjectDisposedException("_notifyIcon");
			_notifyIcon.Visible = true;

			//on first run, add to startup...
			if (Properties.Settings.Default.FirstRun)
			{
				Properties.Settings.Default.FirstRun = false;
				Properties.Settings.Default.Save();
				RegistryKey startupKeys = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				startupKeys.SetValue(Application.ProductName, Application.ExecutablePath);
			}

			//if salts haven't been set yet, set them...
			if (Properties.Settings.Default.CheckSalt == "" || Properties.Settings.Default.CryptSalt == "")
				ResetSalts();

			//if no master password has been set, start the onboarding guide...
			if (Properties.Settings.Default.MasterCheckHash == "")
			{
				SendToast("Welcome to FingerPass!", "Please set a master password to get started.");
				new SetMasterPasswordForm().ShowDialog();

				//if they didn't set a password, we should just quit...
				if (Properties.Settings.Default.MasterCheckHash == "")
				{
					SendToast("FingerPass", "You need to set a master password to use FingerPass. Quitting for now. Please try again!");
					Dispose();
					Application.Exit();
					Environment.Exit(0);
				}
				else
				{
					SendToast("FingerPass", "Cool! Now your passwords can be kept safe. Check out the basic program settings next...");
					new SettingsForm().ShowDialog();

					if (Program.keyDatabase.database.Passwords.Count == 0)
					{
						SendToast("FingerPass", "Nice! Finally, here's your password database. Safely store passwords here and fill them into other programs using your fingerprint.");
						new DatabaseForm().ShowDialog();
					}

					SendToast("FingerPass", "All done! FingerPass will stay in the tray now. Right click the fingerprint icon to get back to the settings and password database. Later!");
				}
			}

			_notifyIcon.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
		}

		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			if (onToastClick != null)
			{
				onToastClick();
			}
		}

		/// <summary>
		/// Dispose of underlying <see cref="NotifyIcon"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose of underlying <see cref="NotifyIcon"/>.
		/// </summary>
		protected virtual void Dispose(bool isDisposing)
		{
			if (!_isDisposed)
			{
				if (isDisposing)
				{
					Publish(new TrayClose());
					bus.Dispose();
					_notifyIcon.Dispose();
				}

				_isDisposed = true;
			}
		}

		/// <summary>
		/// Sets up the tray icon
		/// </summary>
		/// <returns>The resulting NotifyIcon</returns>
		private NotifyIcon CreateNotifyIcon()
		{
			var notifyIcon = new NotifyIcon();

			notifyIcon.DoubleClick += ShowSettings;

			return notifyIcon;
		}

		public bool IsDarkMode()
		{
			isLightMode = true;
			try
			{
				var v = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
				if (v != null && v.ToString() == "0")
					isLightMode = false;
			}
			catch { }

			return isLightMode;
		}

		/// <summary>
		/// Resets the hashing salts (stored with the application's user settings)
		/// </summary>
		/// <param name="save">Whether or not to immediately save the new salts to disk</param>
		private void ResetSalts(bool save = true)
		{
			Properties.Settings.Default.CryptSalt = Security.GenerateRandomString();
			Properties.Settings.Default.CheckSalt = Security.GenerateRandomString();
			
			if (save) 
				Properties.Settings.Default.Save();
		}



		private void LockDatabase(object sender, EventArgs e)
		{
			Program.keyDatabase.Lock();
		}

		private void UnlockDatabase(object sender, EventArgs e)
		{
			try
			{
				Program.keyDatabase.GetMasterCryptHash();
			}
			catch(Exception)
			{
				//ignore cancelled password prompt
			}
		}

		private void Disable(object sender, EventArgs e)
		{
			Publish(new Pause());
		}

		private void Enable(object sender, EventArgs e)
		{
			Publish(new Resume());
		}

		private void ShowDatabase(object sender, EventArgs e)
		{
			if (Application.OpenForms.Count == 0)
			{
				new DatabaseForm().ShowDialog();
			}
		}

		private void ShowSettings(object sender, EventArgs e)
		{
			if (Application.OpenForms.Count == 0)
			{
				new SettingsForm().ShowDialog();
			}
		}

		private void ShowHelp(object sender = null, EventArgs e = null)
		{
			System.Diagnostics.Process.Start("https://github.com/gazugafan/fingerpass");
		}

		private void ShowAbout(object sender, EventArgs e)
		{
			if (Application.OpenForms.Count == 0)
			{
				new About().ShowDialog();
			}
		}

		public void SendToast(string heading, string message, ToastClickDelegate clickEvent = null)
		{
			onToastClick = clickEvent;
			_notifyIcon.ShowBalloonTip(0, heading, message, ToolTipIcon.Info);
		}

		/// <summary>
		/// Flashes the tray icon to red for a moment, and then returns to what it was
		/// </summary>
		public void FlashRed()
		{
			_iconFlash = _iconRed;
			_notifyIcon.Icon = _iconFlash;
			_flashTimer.Stop();
			_flashTimer.Start();
		}

		/// <summary>
		/// Flashes the tray icon to green for a moment, and then returns to what it was
		/// </summary>
		public void FlashGreen()
		{
			_iconFlash = _iconGreen;
			_notifyIcon.Icon = _iconFlash;
			_flashTimer.Stop();
			_flashTimer.Start();
		}

		private void OnFlashTimer(Object source, ElapsedEventArgs e)
		{
			UpdateStatusIcon();
		}

		/// <summary>
		/// Changes the tray menu to reflect the current status
		/// </summary>
		public void UpdateTrayMenu()
		{
			MenuItem[] items = new MenuItem[]{
				new MenuItem("Manage Passwords", ShowDatabase),
				new MenuItem("-"),
				new MenuItem("Settings", ShowSettings),
				new MenuItem("Help", ShowHelp),
				new MenuItem("About", ShowAbout),
				new MenuItem("-"),
				new MenuItem("Exit", delegate { Dispose(); Application.Exit(); })
			};

			if (Program.keyDatabase.IsUnlocked())
				items = items.Prepend(new MenuItem("Lock Database", LockDatabase)).ToArray();
			else
				items = items.Prepend(new MenuItem("Unlock Database", UnlockDatabase)).ToArray();

			if (this._status == Statuses.Paused)
				items = items.Prepend(new MenuItem("Resume Fingerprinting", Enable)).ToArray();
			else
				items = items.Prepend(new MenuItem("Pause Fingerprinting", Disable)).ToArray();

			ContextMenu menu = new ContextMenu(items);
			_notifyIcon.ContextMenu = menu;
		}

		/// <summary>
		/// Changes the tray icon color and text to reflect the current status
		/// </summary>
		public void UpdateStatusIcon()
		{
			if (this._status == Statuses.Listening || this._status == Statuses.Starting)
			{
				if (Program.keyDatabase.IsUnlocked())
				{
					_notifyIcon.Icon = _iconNormal;
					_notifyIcon.Text = "FingerPass";
				}
				else
				{
					_notifyIcon.Icon = _iconGrey;
					_notifyIcon.Text = "FingerPass (Locked)";
				}
			}
			else if(this._status == Statuses.Failing)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (service keeps failing)";
			}
			else if(this._status == Statuses.NoDevice)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (no fingerprint sensor found)";
			}
			else if(this._status == Statuses.NoSession)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (could not start biometric session)";
			}
			else if(this._status == Statuses.NoFocus)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (could not aquire sensor focus)";
			}
			else if(this._status == Statuses.CommunicationFailure)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (can't communicate with service)";
			}
			else if(this._status == Statuses.ServiceStopped)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (service stopped)";
			}
			else if(this._status == Statuses.Paused)
			{
				_notifyIcon.Icon = _iconRed;
				_notifyIcon.Text = "FingerPass (paused)";
			}
		}

		/// <summary>
		/// Sets a new status and updates the tray icon
		/// </summary>
		/// <param name="status">The new status</param>
		public void UpdateStatus(Statuses status)
		{
			if (this._status != status)
			{
				this._status = status;
				this.UpdateStatusIcon();
				this.UpdateTrayMenu();
			}
		}

		public void Identified(string userID)
		{
			//make sure they are the current user...
			if (userID == Security.GetSID())
			{
				FlashGreen();

				if (onIdentified != null)
				{
					onIdentified();
				}
				else
				{
					try
					{
						if (!Program.keyDatabase.FillPassword())
						{
							_recentProgramName = Windows.GetFocusedApplicationName();
							_recentWindowTitle = Windows.GetFocusedWindowTitle();
							SendToast("FingerPass", "No password was found in your database that matches the current window's program name and title. Click here to create one.", CreateNewPassword);
						}
					}
					catch(Exception ex)
					{
						SendToast("FingerPass", ex.Message);
					}
				}
			}
			else
			{
				FlashRed();
				SendToast("FingerPass", "This fingerprint belongs to a different user.");
			}
		}

		public void Unknown()
		{
			FlashRed();

			if (Properties.Settings.Default.SoundPath != "")
			{
				System.Media.SoundPlayer player = new System.Media.SoundPlayer(String.Format(Properties.Settings.Default.SoundPath, AppDomain.CurrentDomain.BaseDirectory + "Sounds\\"));
				player.Play();
			}
		}

		private void CreateNewPassword()
		{
			if (Application.OpenForms.Count == 0)
			{
				new DatabaseForm(_recentProgramName, _recentWindowTitle).ShowDialog();
			}
		}





		/// <summary>
		/// Publishes a message on the bus, but on its own thread, with a 2 second timeout that will cancel the message
		/// </summary>
		public void Publish<T>(T msg, bool failureToast = false) where T : fastipc.Message.Message
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
				{
					throw ex;
				}

				if (done)
				{
					//if the service was down, get the current service status now...
					if (this._status == Statuses.CommunicationFailure)
					{
						UpdateStatus(Statuses.Listening);
						Publish(new GetStatus());
					}
				}
				else
				{
					UpdateStatus(Statuses.CommunicationFailure);
					cts.Cancel();

					if (failureToast)
						SendToast("Failure", "The FingerPass Windows service did not respond. Is it stopped?");
				}
			});
		}
	}
}
