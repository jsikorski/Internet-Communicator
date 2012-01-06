using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Statuses
{
    [Serializable]
    public class StatusesRequest : IRequest
    {
        public IEnumerable<int> Numbers { get; set; } 
    }
}
