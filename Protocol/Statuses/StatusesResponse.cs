using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Statuses
{
    public class StatusesResponse
    {
        public IEnumerable<KeyValuePair<int, bool>> NumbersStatuses { get; set; }
    }
}
