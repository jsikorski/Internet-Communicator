using System;
using System.Windows;
using Caliburn.Micro;
using Client.Services;
using Protocol.Login;

namespace Client.Features.Login
{
    public class LoginViewModel : Screen
    {
        private readonly IServerConnection _serverConnection;
        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Number) &&
                    !string.IsNullOrEmpty(Password);
            }
        }

        public LoginViewModel(IServerConnection serverConnection)
        {
            base.DisplayName = "Internet communicator";

            _serverConnection = serverConnection;
        }

        public void Login()
        {
            var loginRequest = new LoginRequest()
                                   {
                                       Number = Convert.ToInt32(Number),
                                       PasswordHash = Password
                                   };

            if (_serverConnection.SendLoginRequest(loginRequest).WasSuccessfull)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Incorrect number or password.", "Login error");
            }
        }
    }
}
