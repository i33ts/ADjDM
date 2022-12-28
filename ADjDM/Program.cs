using ADjDM.Properties;
using System;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Security.Permissions;
using System.Net.NetworkInformation;

namespace ADjDM
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }

        public class MyCustomApplicationContext : ApplicationContext
        {
            private NotifyIcon trayIcon;

            public MyCustomApplicationContext()
            {
                // Initialize ContextMenuStrip
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                // User Menu
                ToolStripMenuItem userMenu = new ToolStripMenuItem();
                userMenu.Text = "User";
                userMenu.Image = Resources.user.ToBitmap();
                userMenu.DropDownItems.Add("Check Password Health", Resources.key.ToBitmap(), this.CheckPasswordHealth_Click);
                userMenu.DropDownItems.Add("Check Password Strength", Resources.keylock.ToBitmap(), this.CheckPasswordStrength_Click);
                userMenu.DropDownItems.Add("Show User Information", Resources.info.ToBitmap());
                userMenu.DropDownItems.Add("Show Local Admin Indicator", Resources.admin.ToBitmap(), this.CheckLocalAdmin_Click);
                // Computer Menu
                ToolStripMenuItem computerMenu = new ToolStripMenuItem();
                computerMenu.Text = "Computer";
                computerMenu.Image = Resources.computers.ToBitmap();
                computerMenu.DropDownItems.Add("Check Domain Trust", Resources.domain.ToBitmap(), this.CheckDomainTrust_Click);
                computerMenu.DropDownItems.Add("Check Windows Health", Resources.windows.ToBitmap(), this.CheckWindowsHealth_Click);
                computerMenu.DropDownItems.Add("Check Disks Health", Resources.harddisk.ToBitmap());
                computerMenu.DropDownItems.Add("List System Information", Resources.info.ToBitmap(), this.ListSystemInfo_Click);
                computerMenu.DropDownItems.Add("List Installed Software", Resources.software.ToBitmap());
                // Services Menu
                ToolStripMenuItem servicesMenu = new ToolStripMenuItem();
                servicesMenu.Text = "Services";
                servicesMenu.Image = Resources.services.ToBitmap();
                servicesMenu.DropDownItems.Add("Check Windows Time Health", Resources.time.ToBitmap());
                servicesMenu.DropDownItems.Add("Check Windows Update Health", Resources.windows.ToBitmap());
                servicesMenu.DropDownItems.Add("List Warning/Error Application Logs", Resources.log.ToBitmap());
                servicesMenu.DropDownItems.Add("List Error Application Logs", Resources.log.ToBitmap());
                servicesMenu.DropDownItems.Add("List Warning/Error System Logs", Resources.log.ToBitmap());
                servicesMenu.DropDownItems.Add("List Error System Logs", Resources.log.ToBitmap());
                // Policies Menu
                ToolStripMenuItem policiesMenu = new ToolStripMenuItem();
                policiesMenu.Text = "Policies";
                policiesMenu.Image = Resources.policy.ToBitmap();
                policiesMenu.DropDownItems.Add("List Assigned Policies", Resources.info.ToBitmap());
                policiesMenu.DropDownItems.Add("Check Folder Redirection Health", Resources.roaming.ToBitmap());
                policiesMenu.DropDownItems.Add("Check USB Policy", Resources.usb.ToBitmap());
                policiesMenu.DropDownItems.Add("Check CD Policy", Resources.cd.ToBitmap());
                policiesMenu.DropDownItems.Add("Check Encryption Policy", Resources.encryption.ToBitmap());
                // Connectivity Menu
                ToolStripMenuItem connectivityMenu = new ToolStripMenuItem();
                connectivityMenu.Text = "Connectivity";
                connectivityMenu.Image = Resources.connectivity.ToBitmap();
                connectivityMenu.DropDownItems.Add("Check Internet Health", Resources.internet.ToBitmap(), this.CheckInternetHealth_Click);
                    //Connectivity Sub-Menu
                    ToolStripMenuItem connectivitysubMenu = new ToolStripMenuItem();
                    connectivitysubMenu.Text = "Check Network Health";
                    connectivitysubMenu.Image = Resources.lan.ToBitmap();
                    connectivitysubMenu.DropDownItems.Add("NIC Disconnects", Resources.disconnects.ToBitmap());
                    connectivitysubMenu.DropDownItems.Add("CRC Errors", Resources.crc.ToBitmap());
                    connectivitysubMenu.DropDownItems.Add("Packet Retransmissions", Resources.retransmit.ToBitmap());
                connectivityMenu.DropDownItems.Add(connectivitysubMenu);
                connectivityMenu.DropDownItems.Add("Enable/Disable Proxy", Resources.proxy.ToBitmap());
                connectivityMenu.DropDownItems.Add("Concurrent WiFi/Ethernet Indicator", Resources.multicon.ToBitmap(), this.CheckConcurrent_Click);
                // Assistance Menu
                ToolStripMenuItem assistanceMenu = new ToolStripMenuItem();
                assistanceMenu.Text = "Assistance";
                assistanceMenu.Image = Resources.assistance.ToBitmap();
                assistanceMenu.DropDownItems.Add("Request Remote Assistance", Resources.remoteassist.ToBitmap());
                assistanceMenu.DropDownItems.Add("Send Email", Resources.email.ToBitmap());
                // Add Menus to Main Menu
                contextMenuStrip.Items.Add(userMenu);
                contextMenuStrip.Items.Add(computerMenu);
                contextMenuStrip.Items.Add(servicesMenu);
                contextMenuStrip.Items.Add(policiesMenu);
                contextMenuStrip.Items.Add(connectivityMenu);
                contextMenuStrip.Items.Add(assistanceMenu);
                contextMenuStrip.Items.Add("Exit", null, this.Exit_Click);

                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.appicon,
                    ContextMenuStrip = contextMenuStrip,
                    Visible = true
                };
            }

            void Exit_Click(object sender, EventArgs e)
            {
                trayIcon.Visible = false;
                Application.Exit();
            }

            void CheckPasswordHealth_Click(object sender, EventArgs e)
            {
                User.CheckPasswordHealth();
            }

            void CheckLocalAdmin_Click(object sender, EventArgs e) 
            {
                User.CheckLocalAdmin();
            }

            void CheckPasswordStrength_Click(object sender, EventArgs e)
            {
                // Create From
                Form passForm = new Form();
                passForm.Text = "Check your Password";
                passForm.Icon = Resources.keylock;
                passForm.MinimizeBox = false;
                passForm.MaximizeBox = false;
                passForm.Size = new Size(200, 100);
                passForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                passForm.StartPosition = FormStartPosition.CenterScreen;
                // Create Password TextBox
                TextBox passBox = new TextBox();
                passBox.Text = "";
                passBox.PasswordChar = '*';
                passBox.Location = new Point(40, 0);
                // Create Button Check
                Button checkBtn = new Button();
                checkBtn.Text = "Check";
                checkBtn.Location = new Point(50, 25);
                checkBtn.Visible = true;
                checkBtn.BringToFront();
                checkBtn.Click += CheckBtn_Click;
                // Add controls to From
                passForm.Controls.Add(passBox);
                passForm.Controls.Add(checkBtn);

                passForm.Show();

                void CheckBtn_Click(object sender, EventArgs e)
                {
                    string passToCheck = passBox.Text;
                    passForm.Dispose();
                    User.CheckPasswordStrength(passToCheck);
                }
            }

            void CheckDomainTrust_Click(object sender, EventArgs e) 
            {
                string domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                ExecuteCommandSync(@"NLTEST /SC_VERIFY:" + domain);
            }

            void ListSystemInfo_Click(object sender, EventArgs e) 
            {
                ExecuteCommandSync(@"systeminfo");
            }

            void CheckWindowsHealth_Click(object sender, EventArgs e)
            {
                //Change Application icon to executing gif
                SetBalloonTip("Windows Health Check", "This process might take a while... \nWhen it is finished, a message box will come up with the health check results!");
                trayIcon.ShowBalloonTip(1500);
                ExecuteCommandAsync(@"sfc /scannow");
            }

            void CheckInternetHealth_Click(object sender, EventArgs e) 
            {
                Connectivity.CheckInternetHealth();
            }

            void CheckConcurrent_Click(object sender, EventArgs e) 
            {
                Connectivity.CheckConcurrentNICsUp();
            }

            /// <span class="code-SummaryComment"><summary></span>
            /// Executes a shell command synchronously.
            /// <span class="code-SummaryComment"></summary></span>
            /// <span class="code-SummaryComment"><param name="command">string command</param></span>
            /// <span class="code-SummaryComment"><returns>string, as output of the command.</returns></span>
            public void ExecuteCommandSync(object command)
            {
                try
                {
                    // create the ProcessStartInfo using "cmd" as the program to be run,
                    // and "/c " as the parameters.
                    // Incidentally, /c tells cmd that we want it to execute the command that follows,
                    // and then exit.
                    System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.RedirectStandardError = true;
                    procStartInfo.UseShellExecute = false;
                    // Do not create the black window.
                    procStartInfo.CreateNoWindow = true;
                    // Now we create a process, assign its ProcessStartInfo and start it
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                    // Get the output into a string
                    string result = proc.StandardOutput.ReadToEnd();
                    string error = proc.StandardError.ReadToEnd();
                    // Display the command output.
                    if (result != string.Empty)
                    {
                        MessageBox.Show(result);
                    }
                    else
                        MessageBox.Show(error);
                }
                catch (Exception objException)
                {
                    MessageBox.Show(objException.Message);
                }
            }

            /// <span class="code-SummaryComment"><summary></span>
            /// Execute the command Asynchronously.
            /// <span class="code-SummaryComment"></summary></span>
            /// <span class="code-SummaryComment"><param name="command">string command.</param></span>
            public void ExecuteCommandAsync(string command)
            {
                try
                {
                    //Asynchronously start the Thread to process the Execute command request.
                    Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                    //Make the thread as background thread.
                    objThread.IsBackground = true;
                    //Set the Priority of the thread.
                    objThread.Priority = ThreadPriority.AboveNormal;
                    //Start the thread.
                    objThread.Start(command);
                }
                catch (ThreadStartException objException)
                {
                    // Log the exception
                }
                catch (ThreadAbortException objException)
                {
                    // Log the exception
                }
                catch (Exception objException)
                {
                    // Log the exception
                }
            }

            private void SetBalloonTip(string btitle, string btext)
            {
                //optionally change tray icon
                //trayIcon.Icon = SystemIcons.Information;
                trayIcon.BalloonTipTitle = btitle;
                trayIcon.BalloonTipText = btext;
                trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            }
        }
    }
}
