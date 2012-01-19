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
        public IEnumerable<File> Files { get; private set; }

        public FilesDownloadResponse(IEnumerable<File> files)
        {
            Files = files;
        }
    }
}
