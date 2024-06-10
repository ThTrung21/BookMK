using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Service
{
    public static class LoginAttemptHelper
    {
        private static readonly string filePath = "login_attempts.txt";

        public static Dictionary<string, (int FailedAttempts, DateTime LastAttemptTime)> LoadLoginAttempts()
        {
            var loginAttempts = new Dictionary<string, (int, DateTime)>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 && int.TryParse(parts[1], out int attempts) && DateTime.TryParse(parts[2], out DateTime lastAttempt))
                    {
                        loginAttempts[parts[0]] = (attempts, lastAttempt);
                    }
                }
            }

            return loginAttempts;
        }

        public static void SaveLoginAttempts(Dictionary<string, (int FailedAttempts, DateTime LastAttemptTime)> loginAttempts)
        {
            var lines = new List<string>();
            foreach (var kvp in loginAttempts)
            {
                lines.Add($"{kvp.Key},{kvp.Value.FailedAttempts},{kvp.Value.LastAttemptTime}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
