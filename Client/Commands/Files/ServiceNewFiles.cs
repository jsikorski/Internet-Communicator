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
        private readonly IEnumerable<FileHeader> _filesHeaders;
        private readonly IEventAggregator _eventAggregator;

        public ServiceNewFiles(
            IEnumerable<FileHeader> filesHeaders, 
            IEventAggregator eventAggregator)
        {
            _filesHeaders = filesHeaders;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            foreach (var fileHeader in _filesHeaders)
            {
                if (MessageBoxService.ShowQuestion(
                    string.Format("Do you want to download file {0} from user {1}?", 
                    fileHeader.FileName, fileHeader.Sender)) != MessageBoxResult.Yes)
                {
                    continue;
                }

                _eventAggregator.Publish(new FileDownloadAccepted(fileHeader));
            }
        }
    }
}
