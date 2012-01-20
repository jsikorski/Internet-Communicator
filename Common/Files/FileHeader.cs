using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Files
{
    [Serializable]
    public class FileHeader
    {
        public Guid Guid { get; private set; }
        public string FileName { get; private set; }
        public string Sender { get; private set; }

        public FileHeader(Guid guid, string fileName, string sender)
        {
            Guid = guid;
            Sender = sender;
            FileName = fileName;
        }
    }
}
