using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Login
{
    [Serializable]
    public class LoginResponse : IResponse
    {
        public bool WasSuccessfull { get; set; }
    }
}
