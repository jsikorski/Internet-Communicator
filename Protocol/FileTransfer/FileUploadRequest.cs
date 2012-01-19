using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.FileTransfer
{
    [Serializable]
    public class FileUploadRequest : IRequest
    {
        public int Receiver { get; private set; }
        public string OriginalName { get; private set; }
        public byte[] FileBytes { get; private set; }

        public FileUploadRequest(string originalName, byte[] fileBytes, int receiver)
        {
            Receiver = receiver;
            FileBytes = fileBytes;
            OriginalName = originalName;
        }
    }
}
