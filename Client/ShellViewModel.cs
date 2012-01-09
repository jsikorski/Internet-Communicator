using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Login;
using Client.Utils;
using Client.Validators;
using Common.Contacts;

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
            ConnectCommand connectCommand)
        {
            base.DisplayName = "Internet communicator";
            
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
            catch (Exception exception)
            {
                ErrorMessageBox.Show(exception);
                return;
            }

            _windowManager.ShowWindow(_loginViewModel);
            TryClose();
        }
    }
}
