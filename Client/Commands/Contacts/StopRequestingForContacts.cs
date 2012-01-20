using Client.Context;

namespace Client.Commands.Contacts
{
    public class StopRequestingForContacts : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public StopRequestingForContacts(
            ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            _currentContext.GettingContactsTimer.Stop();
        }
    }
}
