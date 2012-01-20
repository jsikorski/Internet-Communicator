using System;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.Connection;
using Client.Features.Login;
using Client.Features.Messages;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;
using Client.Validators;

namespace Client
{
    public class ShellViewModel : Screen, IShell, IBusyScope, IHandle<Connected>
    {
        private readonly IValidator _addressValidator;
        private readonly IWindowManager _windowManager;
        private readonly LoginViewModel _loginViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<string, Connect> _connectFactory;

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

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
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
            IEventAggregator eventAggregator,
            Func<string, Connect> connectFactory)
        {
            base.DisplayName = "Internet communicator";

            _addressValidator = addressValidator;
            _windowManager = windowManager;
            _loginViewModel = loginViewModel;
            _eventAggregator = eventAggregator;
            _connectFactory = connectFactory;

            #if DEBUG
                ServerAddress = "127.0.0.1";
            #endif
        }

        public void Connect()
        {
            _eventAggregator.Subscribe(this);
            ICommand connect = _connectFactory(ServerAddress);
            CommandInvoker.InvokeBusy(connect, this);
        }

        public void Handle(Connected message)
        {
            _eventAggregator.Unsubscribe(this);
            _windowManager.ShowWindow(_loginViewModel);
            TryClose();
        }
    }
}
