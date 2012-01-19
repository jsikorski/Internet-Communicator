using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.FileTransfer
{
    [Serializable]
    class FilesDownloadResponse : IResponse
    {
        public string OriginalName { get; private set; }
        public byte[] FileBytes { get; private set; }

        public FilesDownloadResponse(byte[] fileBytes, string originalName)
        {
            OriginalName = originalName;
            FileBytes = fileBytes;
        }
    }
}
