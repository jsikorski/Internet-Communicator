using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Files
{
    public class File
    {
        public string OriginalName { get; private set; }
        public byte[] FileBytes { get; private set; }
    }
}
