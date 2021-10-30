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
                // Initialize Tray Icon
                trayIcon = new NotifyIcon() { Icon = Resources.appIcon,
                Visible = true };
            }
        }

        //   mainmenu.Items.Add("Exit");

        //     void Exit(object sender, EventArgs e)
        //         {
        // Hide tray icon, otherwise it will remain shown until user mouses over it
        //             trayIcon.Visible = false;

        //             Application.Exit();
        //         }
        //     }
    }
}
