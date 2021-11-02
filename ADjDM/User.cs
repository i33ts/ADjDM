using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace ADjDM
{
    public class User
    {
        public static void CheckPasswordHealth()
        {
            MessageBox.Show("Your passwords health is ok!", "Password Health");
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

            // Test using complexity class

            MessageBox.Show("Your passwords has been pawned: " + hasBeenPawned.ToString(), "Password Strength");
        }

        /*
         * using System.Text;
           using System.Text.RegularExpressions;

            public enum PasswordScore
            {
                Blank = 0,
                VeryWeak = 1,
                Weak = 2,
                Medium = 3,
                Strong = 4,
                VeryStrong = 5
            }

            public class PasswordAdvisor
            {
                public static PasswordScore CheckStrength(string password)
                {
                    int score = 0;

                    if (password.Length < 1)
                        return PasswordScore.Blank;
                    if (password.Length < 4)
                        return PasswordScore.VeryWeak;
                    if (password.Length >= 8)
                        score++;
                    if (password.Length >= 12)
                        score++;
                    if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                        score++;
                    if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
                        Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                        score++;
                    if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                        score++;

                    return (PasswordScore)score;
                }
            }*/

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

    }
}
