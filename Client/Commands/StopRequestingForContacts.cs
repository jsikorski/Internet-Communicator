using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;

namespace Client.Commands
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
