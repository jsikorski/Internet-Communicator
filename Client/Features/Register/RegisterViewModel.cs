using System;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Protocol.Register;

namespace Client.Features.Register
{
    public class RegisterViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly ICommand<Tuple<string, string>> _registerCommand;
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
            RegisterCommand registerCommand)
        {
            base.DisplayName = "Internet communicator";

            _windowManager = windowManager;
            _registerCommand = registerCommand;
        }

        public void Register()
        {
            try
            {
                _registerCommand.Execute(new Tuple<string, string>(Password, PasswordConfirmation));
            }
            catch
            {
                return;
            }

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
