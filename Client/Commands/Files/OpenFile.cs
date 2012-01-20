using System.IO;
using Caliburn.Micro;
using Client.Features.Files;
using Client.Messages;
using Microsoft.Win32;

namespace Client.Commands.Files
{
    public class OpenFile : ICommand
    {
        private readonly IEventAggregator _eventAggregator;

        public OpenFile(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            string filePath = openFileDialog.FileName;
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileName = Path.GetFileName(openFileDialog.FileName);
                byte[] fileBytes = File.ReadAllBytes(filePath);
                var file = new FileBasicInfo(fileName, fileBytes);
                _eventAggregator.Publish(new FileOpened(file));
            }
        }
    }
}
