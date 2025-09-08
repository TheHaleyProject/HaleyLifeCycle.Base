using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkFlowDefinition : IdentityBase {
        public int Version { get; set; }
        public Dictionary<string, string> BaseUrls { get; set; } = new Dictionary<string, string>();
        public TelemetryConfig Telemetry { get; set; }
        public List<WorkFlowPhase> Phases { get; set; } = new List<WorkFlowPhase>();
        public List<WorkFlowStep> Steps { get; set; }
    }
}
