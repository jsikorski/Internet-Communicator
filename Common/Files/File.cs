using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Files
{
    [Serializable]
    public class File
    {
        public string OriginalName { get; private set; }
        public byte[] FileBytes { get; private set; }
        public int SenderNumber { get; private set; }

        public File(string originalName, byte[] fileBytes, int senderNumber)
        {
            SenderNumber = senderNumber;
            FileBytes = fileBytes;
            OriginalName = originalName;
        }
    }
}
