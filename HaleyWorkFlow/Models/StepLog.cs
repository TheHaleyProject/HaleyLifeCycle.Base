using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class StepLog {
        public int StepCode { get; set; }
        public DateTime Timestamp { get; set; }
        public WorkflowStatus Status { get; set; }
        public string Message { get; set; }
    }
}
