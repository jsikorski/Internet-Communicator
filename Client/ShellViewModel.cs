using System;
using System.Windows;
using Caliburn.Micro;
using Client.Features.Communicator;
using Client.Features.Login;
using Client.Services;
using Client.Validators;
namespace Client
{
    public class ShellViewModel : Screen, IShell
    {
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

        private readonly IValidator _addressValidator;
        private readonly IServerConnection _serverConnection;
        private readonly IWindowManager _windowManager;
        private readonly LoginViewModel _loginViewModel;
        private readonly CommunicatorViewModel _communicatorViewModel;

        public ShellViewModel(
            IValidator addressValidator,
            IServerConnection serverConnection,
            IWindowManager windowManager,
            LoginViewModel loginViewModel,
            CommunicatorViewModel communicatorViewModel)
        {
            base.DisplayName = "Internet communicator";

            _addressValidator = addressValidator;
            _serverConnection = serverConnection;
            _windowManager = windowManager;
            _loginViewModel = loginViewModel;
            _communicatorViewModel = communicatorViewModel;
        }

        public void Connect()
        {
            _windowManager.ShowWindow(_communicatorViewModel);
            return;

            try
            {
                _serverConnection.Connect(ServerAddress);
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot connect to server.", "Connection error");
                return;
            }

            _windowManager.ShowWindow(_loginViewModel);
            TryClose();
        }
    }
}
