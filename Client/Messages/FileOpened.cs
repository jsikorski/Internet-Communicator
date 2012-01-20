using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Client.Messages
{
    public class FileOpened
    {
        public File File { get; private set; }

        public FileOpened(File file)
        {
            File = file;
        }
    }
}
