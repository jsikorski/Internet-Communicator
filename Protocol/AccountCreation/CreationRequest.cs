using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.AccountCreation
{
    [Serializable]
    public class CreationRequest : IRequest
    {
        public string Password { get; set; }
    }
}
