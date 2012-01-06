using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Model
{
    public interface IServerConnection
    {
        void Connect(string serverAddress);
    }
}
