using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    public class PathfindingTimeoutException : Exception
    {
        public PathfindingTimeoutException(string message) : base(message) { }
    }
}
