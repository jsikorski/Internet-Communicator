using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Client.Services;
using Client.Utils;
using Microsoft.Win32;
using Protocol.FileTransfer;

namespace Client.Commands
{
    public class UploadFile : ICommand
    {
        private readonly int _receiverNumber;
        private readonly IServerConnection _serverConnection;

        public UploadFile(
            int receiverNumber, 
            IServerConnection serverConnection)
        {
            _receiverNumber = receiverNumber;
            _serverConnection = serverConnection;
        }

        public void Execute()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(openFileDialog.FileName);
            var request = new FileUploadRequest(openFileDialog.FileName, fileBytes, _receiverNumber);
            _serverConnection.SendFileUploadRequest(request);
        }
    }
}
