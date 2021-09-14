using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIManagement
{
    public class QuotaCounter
    {
        public QuotaCounter(DateTime time, long count)
        {
            Timestamp = time;
            TotalRequests = count;
        }
        public DateTime Timestamp { get; private set; }

        public long TotalRequests { get; private set; }
    }
}
