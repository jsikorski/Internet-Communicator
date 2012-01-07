using System;
using System.Windows;
using Caliburn.Micro;
using Client.Features.Connection;

namespace Client
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        public ShellViewModel(ConnectionViewModel connectionViewModel)
        {
            base.DisplayName = "Internet communicator";
            base.ActivateItem(connectionViewModel);
        }
    }
}
