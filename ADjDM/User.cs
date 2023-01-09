using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.DirectoryServices;

namespace ADjDM
{
    public class User
    {
        public static void CheckPasswordHealth()
        {
            string user = WindowsIdentity.GetCurrent().Name;
            string username = user.Split('\\')[1];
            string domain = user.Split('\\')[0];
            try
            {
                DateTime expDate = GetPasswordExpirationDate(username, domain);
                if (expDate != null)
                {
                    if (expDate > DateTime.Now)
                    {
                        MessageBox.Show(("Your password looks healthy.\nYour password expiration date is: " + expDate.ToString()), "ADjDM: Password Health Result");
                    }
                    else 
                    {
                       MessageBox.Show("Your password seems to have \"never expire\" policy enabled!\nYou should contact IT department for further info!", "ADjDM: Password Health Result");
                    }
                }
                else 
                {
                    MessageBox.Show("There is a problem with your password!\nYour password expiration date appears to be empty.\nYou should contact IT department for further info!", "ADjDM: Password Health Result");
                }
            }
            catch 
            {
                MessageBox.Show("There is a problem with your password! Either:\n1.Your password has expired or\n2.Your computer has lost AD trust.\nYou should contact IT department for further info!", "ADjDM: Password Health Result");
            }
        }

        public static void CheckLocalAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                List<Claim> list = new List<Claim>(principal.UserClaims);
                Claim c = list.Find(p => p.Value.Contains("S-1-5-32-544"));
                if (c != null)
                    MessageBox.Show("Current user is Local Admin!", "ADjDM: Check Local Admin");
                else
                    MessageBox.Show("Current user is Not Local Admin!", "ADjDM: Check Local Admin");
            }
        }

        public static void CheckPasswordStrength(string input)
        {
            string pwdHash = GetPasswordHash(input);
            string pwdHashFirst5 = pwdHash.Substring(0, 5);
            string pwdHashRest = pwdHash.Substring(5, (pwdHash.Length - 5));

            var myClient = new WebClient();
            Stream response = myClient.OpenRead("https://api.pwnedpasswords.com/range/" + pwdHashFirst5);
            string pawnedPwds = String.Empty;
            using (var textReader = new StreamReader(response, Encoding.UTF8, true))
            {
                pawnedPwds = textReader.ReadToEnd();
            }
            response.Close();

            // Search for the rest of hash characters in the result
            bool hasBeenPawned = pawnedPwds.Contains(pwdHashRest, StringComparison.OrdinalIgnoreCase);
            int passComplexityScore = CheckComplexity(input);

            // Test using complexity class

            string complexityResult = String.Empty;
            if (passComplexityScore == 0)
                complexityResult = "Very Weak";
            if (passComplexityScore == 1)
                complexityResult = "Weak";
            if (passComplexityScore == 2)
                complexityResult = "Fair";
            if (passComplexityScore == 3)
                complexityResult = "Good";
            if (passComplexityScore == 4)
                complexityResult = "Strong";
            if (passComplexityScore == 5)
                complexityResult = "Excellent";

            if (passComplexityScore > 2 && !hasBeenPawned)
                MessageBox.Show("Your passwords strength is: " + complexityResult + "\nIt hasn't been leaked before.\nIt is safe to keep your password.", "ADjDM: Password Strength");
            if (passComplexityScore <= 2 && !hasBeenPawned)
                MessageBox.Show("Your passwords strength is: " + complexityResult + "\nIt hasn't been leaked before.\nYou should change to a more complex password.", "ADjDM: Password Strength");
            if (passComplexityScore > 2 && hasBeenPawned)
                MessageBox.Show("Your passwords strength is: " + complexityResult + "\nHowever, it has been leaked before!\nYou should change to a more safe password.", "ADjDM: Password Strength");
            if (passComplexityScore <= 2 && hasBeenPawned)
                MessageBox.Show("Your passwords strength is: " + complexityResult + "\nMoreover it has been leaked before!\nYou should change to a more safe password immediately!", "ADjDM: Password Strength");
        }

        private static int CheckComplexity(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return 0;
            if (password.Length < 4)
                return 1;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @".*\d").Success)
                score++;
            if (Regex.Match(password, @".*[a-z]").Success &&
                Regex.Match(password, @".*[A-Z]").Success)
                score++;
            if (Regex.Match(password, @".*[!#$%&'()*+,-./:;<=>?@[\]^_`{|}~]").Success)
                score++;

            return score;
        }

        private static string GetPasswordHash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private static DateTime GetPasswordExpirationDate(string userId, string domainOrMachineName)
        {
            using (var userEntry = new DirectoryEntry("WinNT://" + domainOrMachineName + '/' + userId + ",user"))
            {
                string dateTimeStr = userEntry.InvokeGet("PasswordExpirationDate").ToString();
                return DateTime.Parse(dateTimeStr);
            }
        }
    }
}
