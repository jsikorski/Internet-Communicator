using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;

namespace Client.Commands
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
