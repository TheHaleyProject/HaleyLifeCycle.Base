using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class OnFailureConfig  {
        public string Action { get; set; }
        public Dictionary<string, string> Config { get; set; }
    }
}
