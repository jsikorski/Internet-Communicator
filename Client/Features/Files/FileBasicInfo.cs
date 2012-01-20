using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Features.Files
{
    public class FileBasicInfo
    {
        public string FileName { get; private set; }
        public byte[] FileBytes { get; private set; }

        public FileBasicInfo(string fileName, byte[] fileBytes)
        {
            FileBytes = fileBytes;
            FileName = fileName;
        }
    }
}
