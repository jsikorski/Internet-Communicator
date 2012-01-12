using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Client.Context
{
    public interface ICurrentContext
    {
        Timer GettingContactsTimer { get; set; }
    }
}
