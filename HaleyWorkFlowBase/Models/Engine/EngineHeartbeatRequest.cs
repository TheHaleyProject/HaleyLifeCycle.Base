using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class EngineHeartbeatRequest {
        public string EngineId { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
