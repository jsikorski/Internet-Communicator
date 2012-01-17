using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Insrastructure
{
    public interface IBusyScope
    {
        bool IsBusy { get; set; }
    }
}
