using Client.Context;

namespace Client.Commands.Messages
{
    public class StopRequestingForMessages : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public StopRequestingForMessages(
            ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            _currentContext.GettingMessagesTimer.Stop();
        }
    }
}
