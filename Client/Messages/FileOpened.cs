using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Features.Files;
using Common.Files;

namespace Client.Messages
{
    public class FileOpened
    {
        public FileBasicInfo FileInfo { get; private set; }

        public FileOpened(FileBasicInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
    }
}
