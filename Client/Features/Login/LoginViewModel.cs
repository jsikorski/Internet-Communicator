using System;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.User;
using Client.Features.Communicator;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;
using Protocol.Login;

namespace Client.Features.Login
{
    public class LoginViewModel : Screen, IHandle<Logged>, IBusyScope
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<LoginInformations, Commands.User.Login> _loginFactory;
        private readonly Func<Screen, NewRegister> _newRegisterFactory;
        private readonly IContainer _container;

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

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Number) &&
                    !string.IsNullOrEmpty(Password);
            }
        }

        public LoginViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator,
            Func<LoginInformations, Commands.User.Login> loginFactory,
            Func<Screen, NewRegister> newRegisterFactory, 
            IContainer container)
        {
            base.DisplayName = "Internet communicator";

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _loginFactory = loginFactory;
            _newRegisterFactory = newRegisterFactory;
            _container = container;
        }

        public void Login()
        {
            _eventAggregator.Subscribe(this);
            var loginInformations = new LoginInformations
                                        {
                                            Number = Int32.Parse(Number),
                                            Password = Password
                                        };

            ICommand login = _loginFactory(loginInformations);
            CommandInvoker.InvokeBusy(login, this);
        }

        public void Register()
        {
            ICommand newRegister = _newRegisterFactory(this);
            CommandInvoker.Execute(newRegister);
            TryClose();
        }

        public void Handle(Logged message)
        {
            _eventAggregator.Unsubscribe(this);

            var viewModel = _container.Resolve<CommunicatorViewModel>();
            _windowManager.ShowWindow(viewModel);
            TryClose();
        }
    }
}
