using Client.Features.Files;
using Client.Services;
using Protocol.FileTransfer;

namespace Client.Commands.Files
{
    public class UploadFile : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly int _receiverNumber;
        private readonly FileBasicInfo _fileBasicInfo;

        public UploadFile(
            IServerConnection serverConnection,
            int receiverNumber,
            FileBasicInfo fileBasicInfo)
        {
            _serverConnection = serverConnection;
            _receiverNumber = receiverNumber;
            _fileBasicInfo = fileBasicInfo;
        }

        public void Execute()
        {
            var request = new FileUploadRequest(_fileBasicInfo.FileName, 
                _fileBasicInfo.FileBytes, _receiverNumber);
            _serverConnection.SendFileUploadRequest(request);
        }
    }
}
