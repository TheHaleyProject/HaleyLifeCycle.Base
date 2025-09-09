using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowInstance : IdentityBase {
        public Guid DefinitionId { get; set; }
        public Guid InstanceId { get; set; } //Should be generated from the Database.
        public string Reference { get; set; }
        public Dictionary<string, object> Parameters { get; set; } //Startup parameters
        public Dictionary<string, string> UrlOverrides { get; set; } //Startup overrides (if any)
        public WorkflowStatus State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsSuspended { get; set; }
        public int Environment { get; set; }
    }
}
