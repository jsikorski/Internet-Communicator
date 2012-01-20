using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Protocol.FileTransfer
{
    [Serializable]
    public class FilesDownloadResponse : IResponse
    {
        public IEnumerable<FileHeader> FileHeaders { get; private set; }

        public FilesDownloadResponse(IEnumerable<FileHeader> files)
        {
            FileHeaders = files;
        }
    }
}
