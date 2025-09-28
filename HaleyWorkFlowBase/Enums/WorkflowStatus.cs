using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Enums {
    public enum WorkflowStatus {
        //INITIALIZATION
        [Description("Workflow is created but not yet started. Awaiting dispatch. In case of crash, RESUME.")]
        Pending = 1000,

        //EXECUTION
        [Description("Workflow is actively executing one or more steps. In case of crash, RESUME.")]
        Running = 1001,
        [Description(" Workflow is retrying after a failure (step or group level). In case of crash, RESUME.")]
        Retrying = 1002,
        [Description("Awaiting external input (e.g., user review, webhook callback). In case of crash, HOLD & WAIT.")]
        Waiting = 1003,

        //INTERRUPTION
        [Description("Temporarily halted—often for manual intervention or external signal. In case of crash, HOLD & WAIT.")]
        Paused = 1004,
        [Description("Workflow was bypassed due to conditional logic or configuration. In case of crash, IGNORE.")]
        Skipped = 1005,

        //TERMINATION
        [Description("Termination Manually or programmatically terminated before completion. In case of crash, IGNORE.")]
        Cancelled = 1006,
        [Description("Termination One or more steps failed and recovery was not possible. In case of crash, INSPECT.")]
        Failed = 1007,
        [Description("Termination Workflow exceeded its allowed duration or a critical step timed out. In case of crash, INSPECT.")]
        TimeOut = 1008,
        [Description("Termination All steps have finished successfully. In case of crash, IGNORE.")]
        Completed = 1009
    }
}