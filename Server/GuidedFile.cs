using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Files;

namespace Server
{
    public class GuidedFile
    {
        public GuidedFile(File file, Guid guid)
        {
            Guid = guid;
            File = file;
        }

        public File File { get; private set; }
        public Guid Guid { get; private set; }
    }
}
