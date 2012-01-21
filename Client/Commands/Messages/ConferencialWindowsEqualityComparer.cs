using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Commands.Messages
{
    public class ConferencialWindowsEqualityComparer : IEqualityComparer<IEnumerable<int>>
    {
        public bool Equals(IEnumerable<int> x, IEnumerable<int> y)
        {
            return x.Count() == y.Count() && y.All(x.Contains);
        }

        public int GetHashCode(IEnumerable<int> obj)
        {
            int hashCode = obj.Aggregate(1, (current, i) => current*i);
            hashCode += obj.Count();
            return hashCode;
        }
    }
}
