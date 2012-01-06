using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Validators
{
    public interface IValidator
    {
        bool IsValid(string value);
    }
}
