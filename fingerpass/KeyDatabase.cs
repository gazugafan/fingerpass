using gazugafan.fingerpass.tray;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace gazugafan.fingerpass
{
	class KeyDatabase
	{
		private System.Timers.Timer _sessionTimer = null;
		private string _dbPath = Application.UserAppDataPath + "\\fingerpass.dat";
		private string _masterPasswordHash = null; //hash of the master password stored in memory for the duration of the session. this is used to encrypt/decrypt passwords, and is what renders the database "unlocked"
		public Database database;

		/// <summary>
		/// Opens the user's database, leaving the passwords encrypted.
		/// </summary>
		public KeyDatabase()
		{
			Open();
		}

		/// <summary>
		/// Returns true if it looks like the password database is currently unlocked (there's a master password hash in memory)
		/// </summary>
		/// <returns></returns>
		public bool IsUnlocked()
		{
			return (_masterPasswordHash != null);
		}

		/// <summary>
		/// Attempts to load the user's database into memory, with the passwords left encrypted.
		/// If anything goes wrong, an exception is thrown.
		/// </summary>
		public void Open()
		{
			try
			{
				if (File.Exists(_dbPath))
				{
					//get encrypted passwords...
					database = JsonConvert.DeserializeObject<Database>(File.ReadAllText(_dbPath));
				}
				else
					database = new Database();
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Attempts to save the currently loaded database of encrypted passwords to the db file.
		/// If anything goes wrong, an exception is thrown (but the database is left as-is)
		/// </summary>
		public void Save()
		{
			File.WriteAllText(_dbPath, JsonConvert.SerializeObject(database));
		}

		/// <summary>
		/// Overwrites the password database file with an empty password list and blanks the master password hash
		/// </summary>
		public void Destroy()
		{
			Lock();
			Properties.Settings.Default.MasterCheckHash = "";
			Properties.Settings.Default.Save();
			database = new Database();
			Save();
		}

		/// <summary>
		/// Attempts to import an exported database.
		/// If anything goes wrong, an exception is thrown.
		/// </summary>
		public void Import(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					//get cleartext passwords...
					Database newDatabase = new Database();
					newDatabase = JsonConvert.DeserializeObject<Database>(File.ReadAllText(path));

					//if the passwords are all in cleartext, encrypt them...
					if (newDatabase.Type == "cleartext")
					{
						newDatabase.Type = "encrypted";
						newDatabase.Passwords.ForEach((password) =>
						{
							password.PasswordHash = Protect(password.PasswordHash);
						});

						//save everything...
						database = newDatabase;
						Save();
					}
					else
						throw new Exception("This does not appear to be a FingerPass export file");
				}
				else
					throw new Exception("Could not find file");
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Attempts to save the currently loaded database of encrypted passwords to the db file with passwords decrypted in clear text.
		/// If anything goes wrong, an exception is thrown
		/// </summary>
		public void Export(string path)
		{
			//make a copy of the database with decrypted clear text passwords...
			Database newDatabase = new Database();
			newDatabase.Type = "cleartext";
			database.Passwords.ForEach((password) =>
			{
				Password newDatabasePassword = new Password();
				newDatabasePassword.Program = password.Program;
				newDatabasePassword.Title = password.Title;
				newDatabasePassword.PressEnter = password.PressEnter;
				newDatabasePassword.PasswordHash = password.DecryptedPassword();
				newDatabase.Passwords.Add(newDatabasePassword);
			});

			File.WriteAllText(path, JsonConvert.SerializeObject(newDatabase));

			//TODO: force garbage collection...
			newDatabase = null;
		}

		/// <summary>
		/// If a currentPassword is specified, it is used to decrypt every password, and then encrypts them again using the new password.
		/// Otherwise, the database is destroyed.
		/// In either case, the new password is hashed, saved, the database is unlocked.
		/// <param name="newPassword">The new password</param>
		/// <param name="currentPassword">The current password. If left null, the database is destroyed. If not a match, an exception is thrown</param>
		/// </summary>
		public void UpdatePassword(string newPassword, string currentPassword = null)
		{
			if (currentPassword == null)
			{
				Destroy();
				Properties.Settings.Default.MasterCheckHash = HashCheckPassword(newPassword);
				Properties.Settings.Default.Save();
				Unlock(newPassword);
			}
			else
			{
				if (CheckPassword(currentPassword))
				{
					Unlock(currentPassword);

					//make a copy of the database with decrypted clear text passwords...
					Database newDatabase = new Database();
					database.Passwords.ForEach((password) =>
					{
						Password newDatabasePassword = new Password();
						newDatabasePassword.Program = password.Program;
						newDatabasePassword.Title = password.Title;
						newDatabasePassword.PressEnter = password.PressEnter;
						newDatabasePassword.PasswordHash = password.DecryptedPassword(); //temporarily store in clear text
						newDatabase.Passwords.Add(newDatabasePassword);
					});

					//change the master password and unlock using new master password...
					Properties.Settings.Default.MasterCheckHash = HashCheckPassword(newPassword);
					Unlock(newPassword);

					//encrypt the new database of cleartext passwords using the new master password hash...
					newDatabase.Passwords.ForEach((password) =>
					{
						password.PasswordHash = Protect(password.PasswordHash);
					});

					//save everything...
					database = newDatabase;
					Save();
					Properties.Settings.Default.Save();
				}
				else
					throw new Exception("The current password entered is incorrect.");
			}

		}

		/// <summary>
		/// Checks to see if the specified password matches the stored master password.
		/// </summary>
		/// <param name="password">The password to check</param>
		/// <returns>True or false depending on whether or not the password matches</returns>
		public bool CheckPassword(string password)
		{
			if (password != "" && Properties.Settings.Default.MasterCheckHash != "")
				return (HashCheckPassword(password) == Properties.Settings.Default.MasterCheckHash);

			return false;
		}

		/// <summary>
		/// Returns the specified password hashed for storage check.
		/// </summary>
		/// <param name="password">The password to hash</param>
		/// <returns>The hashed password</returns>
		public string HashCheckPassword(string password)
		{
			byte[] data = System.Text.Encoding.ASCII.GetBytes(password + Properties.Settings.Default.CheckSalt);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			return System.Text.Encoding.ASCII.GetString(data);
		}

		/// <summary>
		/// Returns the specified password hashed for use as encrypting/decrypting salt
		/// If the specified password does not match the storage password, an exception is thrown
		/// </summary>
		/// <param name="password">The password to hash</param>
		/// <returns>The hashed password</returns>
		public string HashCryptPassword(string password)
		{
			if (CheckPassword(password))
			{
				byte[] data = System.Text.Encoding.ASCII.GetBytes(password + Properties.Settings.Default.CryptSalt);
				data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
				return System.Text.Encoding.ASCII.GetString(data);
			}
			else
				throw new Exception("The master password entered is incorrect.");
		}

		/// <summary>
		/// Hashes the specified password, checking to see if it matches. If it does, the hashed password is kept in memory and the session is restarted.
		/// If the password does not match, an exception is thrown.
		/// </summary>
		/// <param name="password"></param>
		public void Unlock(string password)
		{
			this._masterPasswordHash = HashCryptPassword(password);
			RestartSession();

			if (Program.trayIcon != null)
			{
				Program.trayIcon.UpdateTrayMenu();
				Program.trayIcon.UpdateStatusIcon();
			}
		}

		/// <summary>
		/// Restarts the database unlock session timer
		/// </summary>
		public void RestartSession()
		{
			if (_sessionTimer == null)
			{
				_sessionTimer = new System.Timers.Timer();
				_sessionTimer.AutoReset = false;
				_sessionTimer.Elapsed += OnSessionTimer;
			}
			else
			{
				_sessionTimer.Stop();
			}

			_sessionTimer.Interval = Properties.Settings.Default.SessionMinutes * 60 * 1000;
			_sessionTimer.Start();
		}

		private void OnSessionTimer(Object source, ElapsedEventArgs e)
		{
			EndSession();
		}

		/// <summary>
		/// Ends the current session and locks the database.
		/// This is called automatically when a session expires
		/// </summary>
		public void EndSession()
		{
			if (_sessionTimer != null)
				_sessionTimer.Stop();

			Lock();
		}

		/// <summary>
		/// Removes the master password hash from memory
		/// </summary>
		/// <param name="password"></param>
		public void Lock()
		{
			//TODO: Attempt to force garbage collection?
			this._masterPasswordHash = null;

			if (Program.trayIcon != null)
			{
				Program.trayIcon.UpdateTrayMenu();
				Program.trayIcon.UpdateStatusIcon();
			}
		}

		/// <summary>
		/// Closes the database, removes it from memory, and removes the master password hash from memory. 
		/// Does NOT save the database to file first.
		/// Be sure to call this instead of relying on the garbage collector.
		/// </summary>
		public void Close()
		{
			//TODO: Attempt to force garbage collection?
			EndSession();
			database = null;
		}

		/// <summary>
		/// Returns the master password crypt hash.
		/// If this is already in memory, it is returned immediately.
		/// Otherwise, the user is prompted for the master password.
		/// If the password entered does not match, an exception is thrown.
		/// </summary>
		/// <returns></returns>
		public string GetMasterCryptHash()
		{
			if (this._masterPasswordHash != null && this._masterPasswordHash != "")
				return this._masterPasswordHash;

			//show password prompt...
			new MasterPasswordPromptForm().ShowDialog();
			if (this._masterPasswordHash != null && this._masterPasswordHash != "")
				return this._masterPasswordHash;

			throw new Exception("Master password is required to unlock the database");
		}

		/// <summary>
		/// Removes an existing password from the database with the specified program and windowTitle.
		/// Throws an error if the specified program and window title does not exist in the database.
		/// </summary>
		/// <param name="programName,e">The program name that the password belongs to</param>
		/// <param name="windowTitle">The window title that the password belongs to</param>
		public void Remove(string programName, string windowTitle)
		{
			int index = FindPasswordIndex(programName, windowTitle);
			if (index >= 0)
			{
				database.Passwords.RemoveAt(index);
				Save();
			}
			else
				throw new Exception("Cannot remove password because the specified program and window title combination does not exist.");
		}

		/// <summary>
		/// Removes an existing password from the database by index.
		/// Throws an error if the specified index does not exist in the database.
		/// </summary>
		/// <param name="programName,e">The program name that the password belongs to</param>
		/// <param name="windowTitle">The window title that the password belongs to</param>
		public void RemoveIndex(int index)
		{
			if (index >= 0)
			{
				database.Passwords.RemoveAt(index);
				Save();
			}
			else
				throw new Exception("Cannot remove password because the specified index does not exist.");
		}

		/// <summary>
		/// Update the specified password by index. Encrypts the new password and overwrites the entry's program, title, and pressEnter values.
		/// If the program and title already exist somewhere else in the database, an exception is thrown.
		/// Returns the updated password object.
		/// </summary>
		/// <param name="index">The index of the password to update</param>
		/// <param name="programName">The program name that the password belongs to</param>
		/// <param name="windowTitle">The window title that the password belongs to</param>
		/// <param name="pressEnter">Whether or not to automatically press enter after the password is typed</param>
		/// <param name="clearPassword">The password in clear text, which will be encrypted. If left null, the password is not updated.</param>
		public Password UpdateIndex(int index, string programName, string windowTitle, bool pressEnter, string clearPassword = null)
		{
			int existingIndex = FindPasswordIndex(programName, windowTitle);
			if (existingIndex >= 0 && existingIndex != index)
				throw new Exception("Another password already exists with this program name and window title.");

			//make sure index is acceptable...
			if (index >= 0 && index < database.Passwords.Count)
			{
				database.Passwords[index].Program = programName;
				database.Passwords[index].Title = windowTitle;
				database.Passwords[index].PressEnter = pressEnter;

				if (clearPassword != null)
					database.Passwords[index].PasswordHash = Protect(clearPassword);

				Save();

				return database.Passwords[index];
			}
			else
				throw new Exception("The specified index does not exist");
		}

		/// <summary>
		/// Encrypts and adds a new password to the database (or updates an existing one, if the programName and windowTitle already exists)
		/// If oldProgramName and oldProgramTitle are specified, updates this password (possible with changed programName and title). If this password doesn't exist, an exception is thrown.
		/// Returns the added password object.
		/// </summary>
		/// <param name="programName">The program name that the password belongs to</param>
		/// <param name="windowTitle">The window title that the password belongs to</param>
		/// <param name="pressEnter">Whether or not to automatically press enter after the password is typed</param>
		/// <param name="clearPassword">The password in clear text, which will be encrypted</param>
		/// <param name="oldProgramName">If specified along with oldWindowTitle, this entry will be replaced with the new one</param>
		/// <param name="oldWindowTitle">If specified along with oldProgramName, this entry will be replaced with the new one</param>
		public Password AddOrUpdate(string programName, string windowTitle, bool pressEnter, string clearPassword, string oldProgramName = null, string oldWindowTitle = null)
		{
			int index = -1;

			if (oldWindowTitle == null && oldProgramName == null)
			{
				index = FindPasswordIndex(programName, windowTitle);
			}
			else
			{
				index = FindPasswordIndex(oldProgramName, oldWindowTitle);
				if (index == -1)
					throw new Exception("Could not find a password with the specified program name and window title.");
			}

			//if an existing password was found, update it...
			if (index >= 0)
			{
				database.Passwords[index].Program = programName;
				database.Passwords[index].Title = windowTitle;
				database.Passwords[index].PressEnter = pressEnter;
				database.Passwords[index].PasswordHash = Protect(clearPassword);

				Save();

				return database.Passwords[index];
			}
			else //otherwise, add a new password...
			{
				Password password = new Password();
				password.Program = programName;
				password.Title = windowTitle;
				password.PressEnter = pressEnter;
				password.PasswordHash = Protect(clearPassword);
				database.Passwords.Add(password);

				Save();

				return password;
			}
		}

		/// <summary>
		/// Moves the password at the specified index to the new index
		/// </summary>
		/// <param name="indexToMove">The index of the password to move</param>
		/// <param name="newIndex">The new index where you'd like the password to be inserted</param>
		public void MovePassword(int indexToMove, int newIndex)
		{
			Password password = database.Passwords[indexToMove];
			database.Passwords.RemoveAt(indexToMove);
			database.Passwords.Insert(newIndex, password);
			Save();
		}

		/// <summary>
		/// Returns the index of the password containing the specified program and title.
		/// If none exist, returns -1
		/// </summary>
		/// <param name="programName"></param>
		/// <param name="windowTitle"></param>
		public int FindPasswordIndex(string programName, string windowTitle)
		{
			return database.Passwords.FindIndex((password) => {
				return (password.Program == programName && password.Title == windowTitle);
			});
		}

		/// <summary>
		/// Attempts to find a password in the database that matches the specified program name and window title.
		/// Searches the database from the start of the list to the end, and returns the first match.
		/// Note that passwords in the database can contain wildcard characters for loose matching.
		/// Throws an exception if no match can be found.
		/// </summary>
		/// <param name="programName">The full program name to search for</param>
		/// <param name="windowTitle">The full window title to search for</param>
		/// <returns>The matching Password object</returns>
		public Password FindMatchingPassword(string programName, string windowTitle)
		{
			Password result = database.Passwords.Find((password) =>
			{
				return (WildcardMatch(programName, password.Program) && WildcardMatch(windowTitle, password.Title));
			});

			if (result != null)
				return result;
			else
				throw new Exception("Could not find a password in the database with a matching program name and window title.");
		}

		/// <summary>
		/// Attempts to find the index of a password in the database that matches the specified program name and window title.
		/// Searches the database from the start of the list to the end, and returns the first match.
		/// Note that passwords in the database can contain wildcard characters for loose matching.
		/// Throws an exception if no match can be found.
		/// </summary>
		/// <param name="programName">The full program name to search for</param>
		/// <param name="windowTitle">The full window title to search for</param>
		/// <returns>The matching Password index</returns>
		public int FindMatchingPasswordIndex(string programName, string windowTitle)
		{
			int index = database.Passwords.FindIndex((password) =>
			{
				return (WildcardMatch(programName, password.Program) && WildcardMatch(windowTitle, password.Title));
			});

			if (index >= 0)
				return index;
			else
				throw new Exception("Could not find a password in the database with a matching program name and window title.");
		}

		/// <summary>
		/// Tests to see if a string matches a pattern containing * and ? wildcard characters
		/// </summary>
		/// <param name="test">The string to check</param>
		/// <param name="pattern">The pattern to match, which may contain wildcard characters</param>
		/// <returns></returns>
		public static bool WildcardMatch(string test, string pattern)
		{
			if (pattern == "" || pattern == null)
				return true;

			string escaped = "(?i)^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
			return Regex.IsMatch(test, escaped);
		}

		/// <summary>
		/// Searches for a matching password in the database and, if found, decrypts it and types it in.
		/// Returns true if a matching password was found, or false otherwise.
		/// Throws an error if something goes wrong trying to decrypt or type the password.
		/// </summary>
		/// <param name="programName">The program name. Leave null to use the currently focused application's name.</param>
		/// <param name="windowTitle">The window title. Leave null to use the currently focused window's title.</param>
		public bool FillPassword(string programName = null, string windowTitle = null)
		{
			if (programName == null) programName = Windows.GetFocusedApplicationName();
			if (windowTitle == null) windowTitle = Windows.GetFocusedWindowTitle();

			Password password = null;

			try
			{
				password = FindMatchingPassword(programName, windowTitle);
			}
			catch (Exception)
			{
				return false;
			}

			if (password != null)
			{
				password.Type();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Finds the specified password by index, decrypts it, and copies it to the clipboard.
		/// If anything goes wrong, an error is thrown.
		/// </summary>
		public void CopyPassword(int index)
		{
			if (index >= 0 && index < database.Passwords.Count)
			{
				Clipboard.SetText(database.Passwords[index].DecryptedPassword());
			}
			else
				throw new Exception("Can't copy the password because no password exists at that index.");
		}

		/// <summary>
		/// Encrypts the specified clear text with the hashed master password.
		/// Only the currently logged in Windows user can decrypt the resulting encrypted text.
		/// If the database is locked, the user is prompted to enter the master password.
		/// </summary>
		/// <param name="clearText">The text to encrypt</param>
		/// <param name="entropy">The entropy to use (defaults to the hashed master password)</param>
		/// <returns>The encrypted text</returns>
		public string Protect(string clearText, string entropy = null)
		{
			if (clearText == null)
				throw new ArgumentNullException("clearText");

			if (entropy == null)
				entropy = GetMasterCryptHash();

			byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
			byte[] entropyBytes = Encoding.UTF8.GetBytes(entropy);
			byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropyBytes, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encryptedBytes);
		}

		/// <summary>
		/// Decrypts the specified encrypted text with the hashed master password.
		/// Can only decrypt text encrypted by the same currently logged in Windows user.
		/// If the database is locked, the user is prompted to enter the master password.
		/// </summary>
		/// <param name="encryptedText">Text encrypted by the same Windows user</param>
		/// <param name="entropy">The entropy to use (defaults to the hashed master password)</param>
		/// <returns>The decrypted clear text</returns>
		public string Unprotect(string encryptedText, string entropy = null)
		{
			if (encryptedText == null)
				throw new ArgumentNullException("encryptedText");

			if (entropy == null)
				entropy = GetMasterCryptHash();

			byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
			byte[] entropyBytes = Encoding.UTF8.GetBytes(entropy);
			byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, entropyBytes, DataProtectionScope.CurrentUser);
			return Encoding.UTF8.GetString(clearBytes);
		}
	}

	public class Database
	{
		public string Type { get; set; } = "encrypted";
		public int Version { get; set; } = 1;
		public List<Password> Passwords { get; set; }

		public Database()
		{
			Passwords = new List<Password>();
		}
	}

	public class Password
	{
		public string Program { get; set; }
		public string Title { get; set; }
		public string PasswordHash { get; set; }
		public bool PressEnter { get; set; } = true;

		/// <summary>
		/// Returns the decrypted password in clear text
		/// </summary>
		/// <returns></returns>
		public string DecryptedPassword()
		{
			try
			{
				return gazugafan.fingerpass.tray.Program.keyDatabase.Unprotect(this.PasswordHash);
			}
			catch(CryptographicException)
			{
				throw new Exception("There was a problem decrypting this password. Are you sure you're logged in as the same user that encrypted it?");
			}
		}

		/// <summary>
		/// Decrypts and types the password out, possibly followed by pressing ENTER
		/// </summary>
		public void Type()
		{
			//decrypt password and escape special characters that SendKeys uses...
			string password = Regex.Replace(DecryptedPassword(), "[+^%~()]", "{$0}");

			//maybe add an ENTER keypress...
			if (this.PressEnter)
				password += "{ENTER}";

			SendKeys.SendWait(password);
		}
	}
}
