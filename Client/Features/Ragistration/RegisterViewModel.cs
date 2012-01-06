using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Client.Features.Login;
using Client.Services;
using Protocol.AccountCreation;

namespace Client.Features.Ragistration
{
    public class RegisterViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IServerConnection _serverConnection;
        private LoginViewModel _returnViewModel;

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
            IServerConnection serverConnection)
        {
            base.DisplayName = "Internet communicator";

            _windowManager = windowManager;
            _serverConnection = serverConnection;
        }

        public void Register()
        {
            if (Password != PasswordConfirmation)
            {
                MessageBox.Show("Password and password confirmation have to be equal.", "Password confirmation");
                return;
            }

            var registerRequest = new RegisterRequest { Password = Password };
            RegisterResponse registerResponse = _serverConnection.SendRegisterRequest(registerRequest);

            if (registerResponse.WasSuccessfull)
            {
                MessageBox.Show(string.Format("Account was successfully created. Your number is {0}.",
                                              registerResponse.AccountNumber), "Registration completed");
                _returnViewModel.Number = registerResponse.AccountNumber.ToString(CultureInfo.InvariantCulture);
                _returnViewModel.Password = Password;
                _windowManager.ShowWindow(_returnViewModel);
                TryClose();
            }
            else
            {
                MessageBox.Show("Cannot create new account.", "Registration error");
            }
        }

        public void Return()
        {
            _windowManager.ShowWindow(_returnViewModel);
            TryClose();
        }

        public void SetReturnViewModel(LoginViewModel returnViewModel)
        {
            _returnViewModel = returnViewModel;
        }
    }
}
