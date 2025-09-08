using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowPhase {
        public int Code { get; set; }
        public string Name { get; set; }
        public List<int> Steps { get; set; }
        public RetryConfig Retries { get; set; }
        public RetryMode RetryMode { get; set; }
        public LoopConfig Loop { get; set; } // Optional
    }
}
