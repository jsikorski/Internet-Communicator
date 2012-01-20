using System.Windows;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Registration;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;

namespace Client.Features.Register
{
    public class RegisterViewModel : Screen, IBusyScope, IHandle<Registered>
    {
        private readonly IWindowManager _windowManager;
        private readonly IContainer _container;
        private readonly IEventAggregator _eventAggregator;
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
            IContainer container,
            IEventAggregator eventAggregator)
        {
            base.DisplayName = "Internet communicator";

            _windowManager = windowManager;
            _container = container;
            _eventAggregator = eventAggregator;
        }

        public void Register()
        {
            _eventAggregator.Subscribe(this);
            var registerInfomations = new RegisterInformations(Password, PasswordConfirmation);
            ICommand register = _container.Resolve<Commands.User.Register>(new UniqueTypeParameter(registerInfomations));
            CommandInvoker.InvokeBusy(register, this);
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

        public void Handle(Registered message)
        {
            _eventAggregator.Unsubscribe(this);
            MessageBox.Show(string.Format("Account was successfully created. Your number is {0}.",
                                          message.AccountNumber), "Registration completed");
            _windowManager.ShowWindow(_returnViewModel);
            TryClose();
        }
    }
}
