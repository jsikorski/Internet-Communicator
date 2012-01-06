using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Validators
{
    public class AddressValidator : IValidator
    {
        public bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value) && 
                value.Count(c => c == '.') == 3 && 
                !value.EndsWith(".");
        }
    }
}
