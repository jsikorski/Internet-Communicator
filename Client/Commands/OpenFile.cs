using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Messages;
using Common.Files;
using Microsoft.Win32;

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
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var file = new File(filePath, fileBytes, 0);
                _eventAggregator.Publish(new FileOpened(file));
            }
        }
    }
}
