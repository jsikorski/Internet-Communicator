using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class LoginRequest
    {
        public int Number { get; set; }
        public string PasswordHash { get; set; }
    }
}
