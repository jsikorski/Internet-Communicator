using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Messages
{
    public class Registered
    {
        public int AccountNumber { get; private set; }

        public Registered(int accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}
