using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Client.Messages
{
    public class FileDownloadAccepted
    {
       public FileHeader FileHeader { get; private set; }

       public FileDownloadAccepted(FileHeader fileHeader)
       {
           FileHeader = fileHeader;
       }
    }
}
