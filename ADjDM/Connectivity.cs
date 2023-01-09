using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows.Forms;

namespace ADjDM
{
    public class Connectivity
    {
        public static void CheckInternetHealth()
        {
            bool canPing = false;
            bool canResolve = false;
            canPing = PingHost("8.8.8.8");
            canResolve = DoGetHostEntry("www.google.com");
            if (canPing && canResolve)
            {
                MessageBox.Show("Internet works Great!", "ADjDM: Internet Check Successful");
            }
            else if (canPing && !canResolve)
            {
                MessageBox.Show("DNS Error!", "ADjDM: Internet Check Failed");
            }
            else if (!canPing && canResolve)
            {
                MessageBox.Show("Ping Error!", "ADjDM: Internet Check Failed");
            }
            else 
            {
                MessageBox.Show("Generic Error! No Internet Connection Present.", "ADjDM: Internet Check Failed");
            }
        }

        public static void CheckConcurrentNICsUp() 
        {
            string NICsUP = String.Empty;
            NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkCard in networkCards) 
            {
                if (networkCard.OperationalStatus == OperationalStatus.Up) 
                {
                    NICsUP += " - " + networkCard.Description + "\n";
                }
            }
            MessageBox.Show(NICsUP, "ADjDM: Connected NICs");
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        public static bool DoGetHostEntry(string hostname)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(hostname);
                if (host != null) 
                { return true; }
                else { return false; }
            }
            catch 
            {
                return false;
            }
        }
    }
}
