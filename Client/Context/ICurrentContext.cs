using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Client.Features.Messages;
using Timer = System.Timers.Timer;

namespace Client.Context
{
    public interface ICurrentContext
    {
        Timer GettingContactsTimer { get; set; }
        Timer GettingMessagesTimer { get; set; }
        Timer GettingFilesTimer { get; set; }
        Dictionary<int, MessagesViewModel> MessageWindows { get; }
        int LoggedUserNumber { get; set; }
    }
}
