using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class EngineRegistrationRequest {
        public string EngineId { get; set; } = default!;
        public string Environment { get; set; } = default!;
    }
}
