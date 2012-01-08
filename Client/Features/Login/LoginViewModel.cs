using System;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Protocol.Login;

namespace Client.Features.Login
{
    public class LoginViewModel : Screen
    {
        private readonly ICommand<LoginRequest> _loginCommand;
        private readonly ICommand<Screen> _newRegisterCommand;

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

        public LoginViewModel(
            LoginCommand loginCommand, 
            NewRegisterCommand newRegisterCommand)
        {
            base.DisplayName = "Internet communicator";

            _loginCommand = loginCommand;
            _newRegisterCommand = newRegisterCommand;
        }

        public void Login()
        {
            var loginRequest = new LoginRequest()
            {
                Number = Convert.ToInt32(Number),
                PasswordHash = Password
            };

            try
            {
                _loginCommand.Execute(loginRequest);
            }
            catch
            {
                return;
            }
        }

        public void Register()
        {
            _newRegisterCommand.Execute(this);
            TryClose();
        }
    }
}
