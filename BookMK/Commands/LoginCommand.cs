using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookMK.Models;
//using BookMK.Windows;
using BookMK.ViewModels;
using BookMK.Windows;
using MongoDB.Driver;
using Serilog;
using BookMK.Service;
using BookMK.Commands;

namespace BookMK.Commands
{
    public class LoginCommand : CommandBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(LoginCommand));

        private readonly LoginViewModel _loginViewModel;

        private static Dictionary<string, (int FailedAttempts, DateTime LastAttemptTime)> loginAttempts;

        private static readonly int MaxFailedAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        static LoginCommand()
        {
            loginAttempts = LoginAttemptHelper.LoadLoginAttempts();
        }

        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool IsLockedOut(string username)
        {
            if (loginAttempts.ContainsKey(username))
            {
                var (failedAttempts, lastAttemptTime) = loginAttempts[username];
                if (failedAttempts >= MaxFailedAttempts && DateTime.Now - lastAttemptTime < LockoutDuration)
                {
                    return true;
                }
                if (DateTime.Now - lastAttemptTime >= LockoutDuration)
                {
                    // Reset after lockout duration has passed
                    loginAttempts[username] = (0, DateTime.Now);
                    LoginAttemptHelper.SaveLoginAttempts(loginAttempts);
                }
            }
            return false;
        }

        public override void Execute(object parameter)
        {
            _logger.Information("Login command execution started");

            string username = _loginViewModel.Username;
            string password = _loginViewModel.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your username and password!!!");
                _logger.Warning("Login attempt with empty username or password");
                return;
            }

            if (IsLockedOut(username))
            {
                MessageBox.Show("Too many failed login attempts. Please try again later.");
                _logger.Warning("User {Username} is locked out due to too many failed login attempts", username);
                return;
            }

            try
            {
                _logger.Information("Hashing password for user {Username}", username);
                string hashedPassword = HashPassword(password);
                _logger.Information("Password hashed successfully for user {Username}", username);

                DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                _logger.Information("Querying database for user {Username}", username);

                FilterDefinition<Staff> filterLogin = Builders<Staff>.Filter.Where(s => s.Username == username && s.PasswordHash == hashedPassword);
                var matchingStaff = db.ReadFiltered(filterLogin);

                if (matchingStaff != null && matchingStaff.Count > 0)
                {
                    // Reset failed login attempts on successful login
                    loginAttempts[username] = (0, DateTime.Now);
                    LoginAttemptHelper.SaveLoginAttempts(loginAttempts);

                    Staff loggedInStaff = matchingStaff[0];
                    _logger.Information("User {Username} logged in successfully", username);

                    Window f = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    f.Hide();
                    DashBoardWindow dashboard = new DashBoardWindow(loggedInStaff);
                    dashboard.ShowDialog();
                    f.Close();
                }
                else
                {
                    if (loginAttempts.ContainsKey(username))
                    {
                        loginAttempts[username] = (loginAttempts[username].FailedAttempts + 1, DateTime.Now);
                    }
                    else
                    {
                        loginAttempts[username] = (1, DateTime.Now);
                    }

                    LoginAttemptHelper.SaveLoginAttempts(loginAttempts);

                    _logger.Warning("Incorrect password attempt for user {Username}. Attempt {AttemptNumber}", username, loginAttempts[username].FailedAttempts);
                    MessageBox.Show("Incorrect password!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during login attempt for user {Username}", username);
                MessageBox.Show("An error occurred while trying to log in. Please try again later.");
            }

            _logger.Information("Login command execution ended");
        }
    }
}


//private static readonly ILogger _logger = Log.ForContext(typeof(LoginCommand));
//private readonly LoginViewModel _loginViewModel;

//private static Dictionary<string, (int FailedAttempts, DateTime LastAttemptTime)> loginAttempts = new Dictionary<string, (int, DateTime)>();
//private static readonly int MaxFailedAttempts = 5;
//private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

//public LoginCommand(LoginViewModel loginViewModel)
//{
//    _loginViewModel = loginViewModel;
//}

//public override bool CanExecute(object parameter)
//{
//    return base.CanExecute(parameter);
//}

//public static string HashPassword(string password)
//{
//    using (SHA256 sha256 = SHA256.Create())
//    {
//        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//    }
//}
//private bool IsLockedOut(string username)
//{
//    if (loginAttempts.ContainsKey(username))
//    {
//        var (failedAttempts, lastAttemptTime) = loginAttempts[username];
//        if (failedAttempts >= MaxFailedAttempts && DateTime.Now - lastAttemptTime < LockoutDuration)
//        {
//            return true;
//        }
//        if (DateTime.Now - lastAttemptTime >= LockoutDuration)
//        {
//            // Reset after lockout duration has passed
//            loginAttempts[username] = (0, DateTime.Now);
//        }
//    }
//    return false;
//}




//public override void Execute(object parameter)
//{
//    _logger.Information("Login command execution started");

//    string username = _loginViewModel.Username;
//    string password = _loginViewModel.Password;

//    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//    {
//        MessageBox.Show("Please enter your username and password!!!");
//        _logger.Warning("Login attempt with empty username or password");
//        return;
//    }

//    if (IsLockedOut(username))
//    {
//        MessageBox.Show("Too many failed login attempts. Please try again later.");
//        _logger.Warning("User {Username} is locked out due to too many failed login attempts", username);
//        return;
//    }


//    try
//    {
//        _logger.Information("Hashing password for user {Username}", username);
//        string hashedPassword = HashPassword(password);
//        _logger.Information("Password hashed successfully for user {Username}", username);

//        DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
//        _logger.Information("Querying database for user {Username}", username);

//        FilterDefinition<Staff> filterLogin = Builders<Staff>.Filter.Where(s => s.Username == username && s.PasswordHash == hashedPassword);
//        var matchingStaff = db.ReadFiltered(filterLogin);

//        if (matchingStaff != null && matchingStaff.Count > 0)
//        {
//            // Reset failed login attempts on successful login
//            loginAttempts[username] = (0, DateTime.Now);

//            Staff loggedInStaff = matchingStaff[0];
//            _logger.Information("User {Username} logged in successfully", username);

//            Window f = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
//            f.Hide();
//            _logger.Information("Navigate user {Username} to main window", username);
//            DashBoardWindow dashboard = new DashBoardWindow(loggedInStaff);
//            dashboard.ShowDialog();
//            f.Close();
//        }
//        else
//        {
//            if (loginAttempts.ContainsKey(username))
//            {
//                loginAttempts[username] = (loginAttempts[username].FailedAttempts + 1, DateTime.Now);
//            }
//            else
//            {
//                loginAttempts[username] = (1, DateTime.Now);
//            }

//            _logger.Warning("Incorrect password attempt for user {Username}. Attempt {AttemptNumber}", username, loginAttempts[username].FailedAttempts);
//            MessageBox.Show("Incorrect password!");
//        }
//    }
//    catch (Exception ex)
//    {
//        _logger.Error(ex, "An error occurred during login attempt for user {Username}", username);
//        MessageBox.Show("An error occurred while trying to log in. Please try again later.");
//    }

//    _logger.Information("Login command execution ended");