using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowDefinition : IdentityBase {
        public int Version { get; set; }
        public Dictionary<string, string> BaseUrls { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        public TelemetryConfig Telemetry { get; set; }
        public List<WorkflowPhase> Phases { get; set; } = new List<WorkflowPhase>();
        public List<WorkflowStep> Steps { get; set; }
        public object RawJson { get; set; }
    }
}
