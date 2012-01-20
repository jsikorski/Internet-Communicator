using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Insrastructure;

namespace Client.Features.Files
{
    public class DownloadFileViewModel : Screen, IBusyScope
    {
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

        public DownloadFileViewModel()
        {
            base.DisplayName = "Downloading";
        }
    }
}
