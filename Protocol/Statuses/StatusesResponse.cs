using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Statuses
{
    [Serializable]
    public class StatusesResponse : IResponse
    {
        public IEnumerable<KeyValuePair<int, bool>> NumbersStatuses { get; set; }
    }
}
