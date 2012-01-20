using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Messages;
using Microsoft.Win32;
using File = Common.Files.File;

namespace Client.Commands
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
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                var file = new File(fileName, fileBytes, 0);
                _eventAggregator.Publish(new FileOpened(file));
            }
        }
    }
}
