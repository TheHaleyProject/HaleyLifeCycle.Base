using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowEngineEntity {
        public int Id { get; set; }
        public string Guid { get; set; } = string.Empty;
        public int Environment { get; set; }
        public int Status { get; set; } = 1;            // 1 = Active, 0 = Dead, 2 = Retired
        public DateTime LastBeat { get; set; } = DateTime.UtcNow; 
    }
}
