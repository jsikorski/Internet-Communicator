using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Client.Context
{
    public class CurrentContext : ICurrentContext
    {
        public Timer GettingContactsTimer { get; set; }
    }
}
