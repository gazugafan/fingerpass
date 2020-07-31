using System;
using WinBioNET;
using WinBioNET.Enums;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using gazugafan.fingerpass.messages;
using System.Security.Policy;
using System.Data;

namespace gazugafan.fingerpass.service
{
    public class FingerprintScanner
    {
        public Statuses status = Statuses.Paused;
        public Fingerpass _service;
        private WinBioSessionHandle _session;

        public delegate void IdentifyDelegate(string identity);
        public IdentifyDelegate onIdentify;

        public delegate void UnknownIdentityDelegate();
        public UnknownIdentityDelegate onUnknownIdentity;

        public FingerprintScanner()
        {
        }

        /// <summary>
        /// Checks to see if there is an available fingerprint sensor
        /// If no devices are available, we set the status to NoDevice
        /// </summary>
        public void CheckDevices()
        {
            try
            {
                Debug("Searching for fingerprint sensors...");
                var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
                Debug(string.Format("Found {0} sensors", units.Length));

                // Check if we have a connected fingerprint sensor
                if (units.Length == 0)
                {
                    throw new Exception("No units");
                }
                else
                    UpdateStatus(Statuses.NoSession);
            }
            catch (Exception e)
            {
                Debug("Could not find fingerprint sensor");
                Debug(e.Message);
                UpdateStatus(Statuses.NoDevice);
            }
        }

        public void OpenBiometricSession()
        {
            try
            {
                Debug("Opening session...");
                _session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.System, WinBioSessionFlag.Raw, null, WinBioDatabaseId.Default);
                if (_session.IsValid)
                {
                    Debug("Session opened!");
                    Debug(_session.Value.ToString());
                    UpdateStatus(Statuses.NoFocus);
                }
                else
                    throw new Exception("Invalid session handle");
            }
            catch (Exception e)
            {
                Log("Error trying to open biometric session");
                Debug(e.Message);
                UpdateStatus(Statuses.NoSession);
            }
        }

        public void CloseBiometricSession()
        {
            Debug("Closing session...");

            try
            {
                WinBio.CloseSession(_session);
            }
            catch (Exception e)
            {
                Debug("CloseBiometricSession error: " + e.Message);
            }

            UpdateStatus(Statuses.NoSession);
        }

        public void RestartBiometricSession()
        {
            ReleaseFocus();
            CloseBiometricSession();
            UpdateStatus(Statuses.Starting);
        }

        public void AquireFocus()
        {
            try
            {
                ReleaseFocus();

                Debug("Aquiring focus...");
                WinBioErrorCode result = WinBio.AcquireFocus();
                Debug(result.ToString());
                if (result == WinBioErrorCode.Ok)
                {
                    UpdateStatus(Statuses.Listening);
                }
                else
                    throw new Exception("Non-OK response trying to aquire focus.");
            }
            catch (Exception e)
            {
                Debug("Error aquiring focus");
                Debug(e.Message);
                UpdateStatus(Statuses.NoFocus);
            }
        }

        public bool ReleaseFocus()
        {
            Debug("Releasing focus...");

            try
			{
                WinBioErrorCode result = WinBio.ReleaseFocus();
                Debug(result.ToString());
                if (result == WinBioErrorCode.Ok)
                {
                    UpdateStatus(Statuses.NoFocus);
                }

                return (result == WinBioErrorCode.Ok);
            }
            catch (Exception)
			{
                return false;
			}
        }

        /// <summary>
        /// Attempts to listen for a single fingerprint read.
        /// Returns true on a successful listen, or false if something went wrong
        /// </summary>
        public bool Listen()
        {
            Debug("Identifying user...");
            try
            {
                WinBioIdentity identity;
                WinBioBiometricSubType subFactor;
                WinBioRejectDetail rejectDetail;
                WinBio.Identify(_session, out identity, out subFactor, out rejectDetail);
                Debug(string.Format("Identity: {0}", identity));
                Debug(identity.TemplateGuid.ToString());
                onIdentify(identity.AccountSid.ToString());
                return true;
            }
            catch (WinBioException ex)
            {
                if (ex.ErrorCode == WinBioErrorCode.UnknownID)
                {
                    onUnknownIdentity();
                    return true;
                }

                Debug(ex);
            }
            catch (Exception ex)
            {
                Debug(ex.Message);
            }

            return false;
        }


        /// <summary>
        /// Begins the main processing loop on a new thread.
        /// </summary>
        public void Process()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Statuses priorStatus = this.status;
                int failures = 0;
                while (true)
                {
                    if (this.status != Statuses.Paused)
                    {
                        Debug("Start of main process loop. failures: " + failures.ToString());

                        if (this.status == Statuses.Starting) UpdateStatus(Statuses.NoDevice);
                        if (this.status == Statuses.NoDevice) this.CheckDevices();
                        if (this.status == Statuses.NoSession) this.OpenBiometricSession();
                        if (this.status == Statuses.NoFocus) this.AquireFocus();
                        if (this.status == Statuses.Listening)
                        {
                            //always let the tray know we're listening...
                            priorStatus = Statuses.Listening;
                            SendStatus();

                            if (this.Listen())
                            {
                                failures = 0;
                            }
                            else
                            {
                                failures++;
                                if (status != Statuses.Paused)
                                    RestartBiometricSession();
                            }
                        }
                        else
                            failures++;

                        //send a status update if the status changed...
                        if (this.status != priorStatus)
                        {
                            Debug("Status changed from " + priorStatus.ToString() + " to " + status.ToString() + ". Sending status update...");
                            priorStatus = this.status;
                            SendStatus();
                        }

                        Debug("End of main process loop. failures: " + failures.ToString());

                        //throttle constant failures...
                        if (failures > 10)
                        {
                            Debug("Failing. Will restart and wait 10 seconds...");
                            if (this.status == Statuses.Listening) UpdateStatus(Statuses.Failing);
                            SendStatus();
                            failures = 0;
                            Thread.Sleep(10000);
                            RestartBiometricSession();
                        }
                        else
                            Thread.Sleep(500);
                    }
                    else
                    {
                        //send a status update if the status changed...
                        if (this.status != priorStatus)
                        {
                            Debug("Paused. Waiting...");
                            priorStatus = this.status;
                            SendStatus();
                        }

                        Thread.Sleep(2000);
                    }
                }
            });

            Debug("processing");
        }

        public void Stop()
        {
            UpdateStatus(Statuses.ServiceStopped);
            SendStatus();
            ReleaseFocus();
            CloseBiometricSession();
        }

        public void Pause()
        {
            UpdateStatus(Statuses.Paused);
            ReleaseFocus();
            CloseBiometricSession();
        }

        public void Resume()
        {
            UpdateStatus(Statuses.Starting);
        }

        /// <summary>
        /// Sends the current status to the tray
        /// </summary>
        public void SendStatus()
        {
            Debug("SendStatus: " + this.status.ToString());
            _service.Publish(new StatusUpdate(this.status));
        }

        /// <summary>
        /// Sets a new status, with some restrictions
        /// </summary>
        /// <param name="status">The new status</param>
        public void UpdateStatus(Statuses status)
        {
            if (status != this.status)
            {
                if (this.status != Statuses.ServiceStopped && (this.status != Statuses.Paused || status == Statuses.Starting))
                {
                    Debug("UpdateStatus: from " + this.status.ToString() + " to " + status.ToString());
                    this.status = status;
                }
            }
        }




        protected void Log(string message)
        {
            EventLog.WriteEntry("Fingerpass", message);
        }

        protected void Log(WinBioException exception)
        {
            EventLog.WriteEntry("Fingerpass", exception.Message, EventLogEntryType.Error);
        }

        [Conditional("DEBUG")]
        protected void Debug(string message)
        {
            EventLog.WriteEntry("Fingerpass", message, EventLogEntryType.Warning);
        }

        [Conditional("DEBUG")]
        protected void Debug(WinBioException exception)
        {
            Debug(exception.Message);
        }
    }
}
