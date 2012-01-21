using System.Collections.Generic;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Client.Utils;
using Common.Files;

namespace Client.Commands.Files
{
    public class ServiceNewFiles : ICommand
    {
        private readonly IEnumerable<FileHeader> _filesHeaders;
        private readonly IEventAggregator _eventAggregator;
        private readonly INumbersToNamesConverter _numbersToNamesConverter;

        public ServiceNewFiles(
            IEnumerable<FileHeader> filesHeaders, 
            IEventAggregator eventAggregator,
            INumbersToNamesConverter numbersToNamesConverter)
        {
            _filesHeaders = filesHeaders;
            _eventAggregator = eventAggregator;
            _numbersToNamesConverter = numbersToNamesConverter;
        }

        public void Execute()
        {
            foreach (var fileHeader in _filesHeaders)
            {
                string sender = _numbersToNamesConverter.Convert(fileHeader.Sender);

                if (MessageBoxService.ShowQuestion(
                    string.Format("Do you want to download file {0} from user {1}?", 
                    fileHeader.FileName, sender)) != MessageBoxResult.Yes)
                {
                    continue;
                }

                _eventAggregator.Publish(new FileDownloadAccepted(fileHeader));
            }
        }
    }
}
