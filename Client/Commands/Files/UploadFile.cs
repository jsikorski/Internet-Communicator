using Client.Services;

namespace Client.Commands.Files
{
    public class UploadFile : ICommand
    {
        private readonly int _receiverNumber;
        private readonly IServerConnection _serverConnection;

        public UploadFile(
            int receiverNumber, 
            IServerConnection serverConnection)
        {
            _receiverNumber = receiverNumber;
            _serverConnection = serverConnection;
        }

        public void Execute()
        {
            

            //byte[] fileBytes = File.ReadAllBytes(openFileDialog.FileName);
            //var request = new FileUploadRequest(openFileDialog.FileName, fileBytes, _receiverNumber);
            //_serverConnection.SendFileUploadRequest(request);
        }
    }
}
