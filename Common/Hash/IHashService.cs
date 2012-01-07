using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Hash
{
    public interface IHashService
    {
        string GetHash(string text);
    }
}
