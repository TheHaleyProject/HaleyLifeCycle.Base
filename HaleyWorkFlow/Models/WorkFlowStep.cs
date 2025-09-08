using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Models {
    public class WorkFlowStep {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string BaseUrl { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> QueryParams { get; set; }
        public Dictionary<string, object> Body { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public List<int> DependsOn { get; set; }
        public List<int> DependsOnGroup { get; set; }
        public string Timeout { get; set; }
        public bool Parallel { get; set; }
        public List<string> Tags { get; set; }
        public RetryConfig Retries { get; set; }
        public string SuccessCondition { get; set; }
        public OnFailureConfig OnFailure { get; set; }
        public OnTimeoutConfig OnTimeout { get; set; }
        public bool OnError { get; set; }
        public string Trigger { get; set; }
    }
}
