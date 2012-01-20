using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Client.Messages
{
    public class FilesFounded
    {
        public IEnumerable<FileHeader> Files { get; private set; }

        public FilesFounded(IEnumerable<FileHeader> files)
        {
            Files = files;
        }
    }
}
