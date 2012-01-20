using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.FileTransfer
{
    [Serializable]
    public class FileDownloadRequest : IRequest
    {
        public Guid FileGuid { get; private set; }

        public FileDownloadRequest(Guid fileGuid)
        {
            FileGuid = fileGuid;
        }
    }
}
