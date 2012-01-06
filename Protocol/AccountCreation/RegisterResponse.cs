using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.AccountCreation
{
    [Serializable]
    public class RegisterResponse : IResponse
    {
        public bool WasSuccessfull { get; set; }
        public int AccountNumber { get; set; }
    }
}
