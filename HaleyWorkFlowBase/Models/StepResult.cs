using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class StepResult {
        public WorkflowStatus Status { get; set; } // replaces Success
        public object Output { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string ErrorMessage { get; set; } // optional
        public int? RetryCount { get; set; }
    }
}
