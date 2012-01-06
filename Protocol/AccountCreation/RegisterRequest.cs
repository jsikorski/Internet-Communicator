using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.AccountCreation
{
    [Serializable]
    public class RegisterRequest : IRequest
    {
        public string Password { get; set; }
    }
}
