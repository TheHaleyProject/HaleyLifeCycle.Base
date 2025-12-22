using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class RetryConfig {
        public int Max { get; set; }
        public string Backoff { get; set; }
    }
}
