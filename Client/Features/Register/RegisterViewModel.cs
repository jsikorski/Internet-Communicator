using System;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Client.Utils;
using Protocol.Register;

namespace Client.Features.Register
{
    public class RegisterViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly Commands.Register _register;
        private Screen _returnViewModel;

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private string _passwordConfirmation;
        public string PasswordConfirmation
        {
            get { return _passwordConfirmation; }
            set
            {
                _passwordConfirmation = value;
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool CanRegister
        {
            get
            {
                return !string.IsNullOrEmpty(Password) &&
                       !string.IsNullOrEmpty(PasswordConfirmation);
            }
        }

        public RegisterViewModel(
            IWindowManager windowManager,
            Commands.Register register)
        {
            base.DisplayName = "Internet communicator";

            _windowManager = windowManager;
            _register = register;
        }

        public void Register()
        {
            int accountNumber = _register.Execute(
                new RegisterInformations(Password, PasswordConfirmation));

            MessageBox.Show(string.Format("Account was successfully created. Your number is {0}.",
                                          accountNumber), "Registration completed");

            _windowManager.ShowWindow(_returnViewModel);
            TryClose();
        }

        public void Return()
        {
            _windowManager.ShowWindow(_returnViewModel);
            TryClose();
        }

        public void SetReturnViewModel(Screen returnViewModel)
        {
            _returnViewModel = returnViewModel;
        }
    }
}
