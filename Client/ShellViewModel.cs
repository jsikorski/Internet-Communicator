using System;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Communicator;
using Client.Features.Login;
using Client.Validators;

namespace Client
{
    public class ShellViewModel : Screen, IShell
    {
        private readonly IValidator _addressValidator;
        private readonly IWindowManager _windowManager;
        private readonly LoginViewModel _loginViewModel;
        private readonly ICommand<string> _connectCommand;

        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                NotifyOfPropertyChange(() => CanConnect);
            }
        }

        public bool CanConnect
        {
            get
            {
                return _addressValidator.IsValid(ServerAddress);
            }
        }

        public ShellViewModel(
            IValidator addressValidator,
            IWindowManager windowManager,
            LoginViewModel loginViewModel,
            ConnectCommand connectCommand,
            CommunicatorViewModel communicatorViewModel)
        {
            base.DisplayName = "Internet communicator";
            windowManager.ShowWindow(communicatorViewModel);

            _addressValidator = addressValidator;
            _windowManager = windowManager;
            _loginViewModel = loginViewModel;
            _connectCommand = connectCommand;
        }

        public void Connect()
        {
            try
            {
                _connectCommand.Execute(ServerAddress);
            }
            catch
            {
                return;
            }

            _windowManager.ShowWindow(_loginViewModel);
            TryClose();
        }
    }
}
