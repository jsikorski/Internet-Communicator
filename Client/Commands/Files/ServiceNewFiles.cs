using System.Collections.Generic;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Client.Messages;
using Client.Utils;
using Common.Files;

namespace Client.Commands.Files
{
    public class ServiceNewFiles : ICommand
    {
        private readonly IEnumerable<File> _files;
        private readonly IEventAggregator _eventAggregator;

        public ServiceNewFiles(
            IEnumerable<File> files, 
            IEventAggregator eventAggregator)
        {
            _files = files;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            foreach (var file in _files)
            {
                if (MessageBoxService.ShowQuestion(
                    string.Format("Do you want to download file {0} from user {1}?", 
                    file.OriginalName, file.SenderNumber)) != MessageBoxResult.Yes)
                {
                    continue;
                }

                _eventAggregator.Publish(new FileDownloadAccepted(file));
            }
        }
    }
}
