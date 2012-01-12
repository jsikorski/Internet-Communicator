using System;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Communicator;
using Client.Utils;
using Protocol.Login;

namespace Client.Features.Login
{
    public class LoginViewModel : Screen
    {
        private readonly LoginCommand _loginCommand;
        private readonly NewRegisterCommand _newRegisterCommand;
        private readonly IWindowManager _windowManager;
        private readonly CommunicatorViewModel _communicatorViewModel;

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
            NewRegisterCommand newRegisterCommand,
            IWindowManager windowManager,
            CommunicatorViewModel communicatorViewModel)
        {
            base.DisplayName = "Internet communicator";

            _loginCommand = loginCommand;
            _newRegisterCommand = newRegisterCommand;
            _windowManager = windowManager;
            _communicatorViewModel = communicatorViewModel;
        }

        public void Login()
        {
            var loginInformations = new LoginInformations()
                                        {
                                            Number = Int32.Parse(Number),
                                            Password = Password
                                        };

            _loginCommand.Execute(loginInformations);
            _windowManager.ShowWindow(_communicatorViewModel);
            TryClose();
        }

        public void Register()
        {
            _newRegisterCommand.Execute(this);
            TryClose();
        }
    }
}
