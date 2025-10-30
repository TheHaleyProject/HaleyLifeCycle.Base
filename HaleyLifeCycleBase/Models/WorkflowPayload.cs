using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkflowPayload {
        private Dictionary<string,string>? _urls;
        public Dictionary<string,string> Urls {
            get { return _urls ?? Definition?.BaseUrls; }
            set { _urls = value; }
        }

        private Dictionary<string,object>? _parameters;
        public Dictionary<string,object> Parameters {
            get { return _parameters ?? Definition?.Parameters; }
            set { _parameters = value; }
        }

        public int InstanceSource { get; set; }
        public int Environment { get; set; }
        public long Owner { get; set; }
        public string? Reference { get; set; }
        public WorkflowDefinition Definition { get; set; }
    }
}
