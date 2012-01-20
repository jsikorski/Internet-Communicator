using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Client.Messages
{
    public class FileDownloaded
    {
        public File File { get; private set; }

        public FileDownloaded(File file)
        {
            File = file;
        }
    }
}
