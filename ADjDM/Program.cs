using ADjDM.Properties;
using System;
using System.Windows.Forms;

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
            //  Application.Run(new Form1());
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
                userMenu.DropDownItems.Add("Check Password Health");
                userMenu.DropDownItems.Add("Check Password Strength");
                userMenu.DropDownItems.Add("Show User Information");
                userMenu.DropDownItems.Add("Show Local Admin Indicator");
                // Computer Menu
                ToolStripMenuItem computerMenu = new ToolStripMenuItem();
                computerMenu.Text = "Computer";
                computerMenu.DropDownItems.Add("Check Domain Trust");
                computerMenu.DropDownItems.Add("Check Windows Health");
                computerMenu.DropDownItems.Add("Check Disks Health");
                computerMenu.DropDownItems.Add("List System Information");
                computerMenu.DropDownItems.Add("List Installed Software");
                // Services Menu
                ToolStripMenuItem servicesMenu = new ToolStripMenuItem();
                servicesMenu.Text = "Services";
                servicesMenu.DropDownItems.Add("Check Windows Time Health");
                servicesMenu.DropDownItems.Add("Check Windows Update Health");
                servicesMenu.DropDownItems.Add("List Warning/Error Application Logs");
                servicesMenu.DropDownItems.Add("List Error Application Logs");
                servicesMenu.DropDownItems.Add("List Warning/Error System Logs");
                servicesMenu.DropDownItems.Add("List Error System Logs");
                // Policies Menu
                ToolStripMenuItem policiesMenu = new ToolStripMenuItem();
                policiesMenu.Text = "Policies";
                policiesMenu.DropDownItems.Add("List Assigned Policies");
                policiesMenu.DropDownItems.Add("Check Folder Redirection Health");
                policiesMenu.DropDownItems.Add("Check USB Policy");
                policiesMenu.DropDownItems.Add("Check CD Policy");
                policiesMenu.DropDownItems.Add("Check Encryption Policy");
                // Connectivity Menu
                ToolStripMenuItem connectivityMenu = new ToolStripMenuItem();
                connectivityMenu.Text = "Connectivity";
                connectivityMenu.DropDownItems.Add("Check Internet Health");
                    //Connectivity Sub-Menu
                    ToolStripMenuItem connectivitysubMenu = new ToolStripMenuItem();
                    connectivitysubMenu.Text = "Check Network Health";
                    connectivitysubMenu.DropDownItems.Add("NIC disconnects");
                    connectivitysubMenu.DropDownItems.Add("CRC Errors");
                    connectivitysubMenu.DropDownItems.Add("Packet Retransmissions");
                connectivityMenu.DropDownItems.Add(connectivitysubMenu);
                connectivityMenu.DropDownItems.Add("Enable/Disable Proxy");
                connectivityMenu.DropDownItems.Add("Concurrent WiFi/Ethernet Indicator");
                // Assistance Menu
                ToolStripMenuItem assistanceMenu = new ToolStripMenuItem();
                assistanceMenu.Text = "Assistance";
                assistanceMenu.DropDownItems.Add("Live Chat");
                assistanceMenu.DropDownItems.Add("Send Email");
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
                    Icon = Resources.appIcon,
                    ContextMenuStrip = contextMenuStrip,
                    Visible = true
                };
            }

            void Exit_Click(object sender, EventArgs e)
            {
                trayIcon.Visible = false;
                Application.Exit();
            }

        }
    }
}
