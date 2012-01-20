using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Protocol.FileTransfer
{
    [Serializable]
    public class FileDownloadResponse : IResponse
    {
        public File File { get; private set; }

        public FileDownloadResponse(File file)
        {
            File = file;
        }
    }
}
