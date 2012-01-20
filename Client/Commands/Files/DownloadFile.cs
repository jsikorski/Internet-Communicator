using Common.Files;
using Microsoft.Win32;

namespace Client.Commands.Files
{
    public class DownloadFile : ICommand
    {
        private readonly File _file;

        public DownloadFile(File file)
        {
            _file = file;
        }

        public void Execute()
        {
            var openFileDialog = new OpenFileDialog {FileName = _file.OriginalName};
            openFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }
        }
    }
}
