using System;
using System.ComponentModel;
using System.Windows;
using Caliburn.Micro;
using Client.Model;
using Client.Validators;
using Client.ViewModel;

namespace Client {
    using System.ComponentModel.Composition;

    [Export(typeof(IShell))]
    public class ShellViewModel : ViewModelBase, IShell
    {
        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set 
            {
                _serverAddress = value;
                RaiseChanged(() => CanConnect);
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

        public ShellViewModel(IValidator addressValidator, IServerConnection serverConnection)
        {
            _addressValidator = addressValidator;
            _serverConnection = serverConnection;
        }

        public void Connect()
        {
            try
            {
                _serverConnection.Connect(ServerAddress);
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot connect to server.", "Connection error");
            }
        }
    }
}
