using System.Diagnostics;

namespace BTE_group_net.Core
{
    public class BulkStatics
    {
        public int BatchSize { get; set; }
        public int BatchCount { get; set; }
        public long TotalItems { get; set; }
        public Stopwatch ElapsedTime { get; set; }
        public Exception Exception { get; set; }
        public bool Success { get; set; }
    }
}
