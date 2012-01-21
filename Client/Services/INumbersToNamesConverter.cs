using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Services
{
    public interface INumbersToNamesConverter
    {
        string Convert(int number);
    }
}
