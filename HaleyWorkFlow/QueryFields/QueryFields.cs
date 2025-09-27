using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public partial class QueryFields {
        // WF_PARAMS additions for WORKFLOW
        public const string SOURCE = "@SOURCE";
        public const string ID = "@ID";
        public const string GUID = "@GUID";
        public const string CODE = "@CODE";
        public const string NAME = "@NAME";

        // WF_PARAMS additions for WF_VERSION
        public const string WORKFLOW = "@WORKFLOW";     // FK → WORKFLOW.ID
        public const string VERSION = "@VERSION";         // integer version number
        public const string DEFINITION = "@DEFINITION";      // JSON/text definition

    }
}
