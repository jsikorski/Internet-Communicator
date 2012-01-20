using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Client.Messages
{
    public class FileDownloadAccepted
    {
       public File File { get; private set; }

       public FileDownloadAccepted(File file)
       {
           File = file;
       }
    }
}
