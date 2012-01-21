using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using File = Common.Files.File;

namespace Client.Commands.Files
{
    public class SaveFile : ICommand
    {
        private readonly File _file;

        public SaveFile(File file)
        {
            _file = file;
        }

        public void Execute()
        {
            var saveFileDialog = new SaveFileDialog { FileName = _file.OriginalName };
            saveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            using (var fileWriter = new FileStream(saveFileDialog.FileName, FileMode.Create))
            {
                fileWriter.Write(_file.FileBytes, 0, _file.FileBytes.Count());
            }
        }
    }
}
