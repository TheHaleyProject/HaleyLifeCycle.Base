using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowState {
        public WorkflowStatus Status { get; set; } // overall workflow status
        public int CurrentPhaseCode { get; set; }
        public int CurrentStepCode { get; set; }
        public Dictionary<int, StepResult> StepResults { get; set; }
        public Dictionary<string, object> RuntimeContext { get; set; }
        public List<StepLog> Logs { get; set; }
    }
}
