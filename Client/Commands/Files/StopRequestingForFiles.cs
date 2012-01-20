using Client.Context;

namespace Client.Commands.Files
{
    public class StopRequestingForFiles : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public StopRequestingForFiles(ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            _currentContext.GettingFilesTimer.Stop();
        }
    }
}
