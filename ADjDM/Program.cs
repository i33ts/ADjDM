using ADjDM.Properties;
using System;
using System.Windows.Forms;
using System.Net;

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
                userMenu.DropDownItems.Add("Show Local Admin Indicator", Resources.admin.ToBitmap());
                // Computer Menu
                ToolStripMenuItem computerMenu = new ToolStripMenuItem();
                computerMenu.Text = "Computer";
                computerMenu.Image = Resources.computers.ToBitmap();
                computerMenu.DropDownItems.Add("Check Domain Trust", Resources.domain.ToBitmap());
                computerMenu.DropDownItems.Add("Check Windows Health", Resources.windows.ToBitmap());
                computerMenu.DropDownItems.Add("Check Disks Health", Resources.harddisk.ToBitmap());
                computerMenu.DropDownItems.Add("List System Information", Resources.info.ToBitmap());
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
                connectivityMenu.DropDownItems.Add("Check Internet Health", Resources.internet.ToBitmap());
                    //Connectivity Sub-Menu
                    ToolStripMenuItem connectivitysubMenu = new ToolStripMenuItem();
                    connectivitysubMenu.Text = "Check Network Health";
                    connectivitysubMenu.Image = Resources.lan.ToBitmap();
                    connectivitysubMenu.DropDownItems.Add("NIC Disconnects", Resources.disconnects.ToBitmap());
                    connectivitysubMenu.DropDownItems.Add("CRC Errors", Resources.crc.ToBitmap());
                    connectivitysubMenu.DropDownItems.Add("Packet Retransmissions", Resources.retransmit.ToBitmap());
                connectivityMenu.DropDownItems.Add(connectivitysubMenu);
                connectivityMenu.DropDownItems.Add("Enable/Disable Proxy", Resources.proxy.ToBitmap());
                connectivityMenu.DropDownItems.Add("Concurrent WiFi/Ethernet Indicator", Resources.multicon.ToBitmap());
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

            void CheckPasswordStrength_Click(object sender, EventArgs e)
            {
                User.CheckPasswordStrength(CredentialCache.DefaultNetworkCredentials.Password);
            }

        }
    }
}
