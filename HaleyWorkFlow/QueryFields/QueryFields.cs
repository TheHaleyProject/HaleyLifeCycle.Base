using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public partial class QueryFields {
        // Common
        public const string ID = "@ID";
        public const string CREATED_AT = "@CREATED_AT";
        public const string CREATED_BY = "@CREATED_BY";
        public const string UPDATED_AT = "@UPDATED_AT";
        public const string UPDATED_BY = "@UPDATED_BY";

        // Definition
        public const string DEF_ID = "@DEF_ID";
        public const string VERSION = "@VERSION";
        public const string NAME = "@NAME";
        public const string DESCRIPTION = "@DESCRIPTION";

        // Instance
        public const string INST_ID = "@INST_ID";
        public const string STATUS = "@STATUS";
        public const string PARAMETERS = "@PARAMETERS";
        public const string STARTED_AT = "@STARTED_AT";
        public const string COMPLETED_AT = "@COMPLETED_AT";

        // Step
        public const string STEP_ID = "@STEP_ID";
        public const string STEP_NAME = "@STEP_NAME";
        public const string STEP_ORDER = "@STEP_ORDER";
        public const string PHASE = "@PHASE";
        public const string CONFIG = "@CONFIG";

        // Log
        public const string LOG_ID = "@LOG_ID";
        public const string RESULT = "@RESULT";
        public const string MESSAGE = "@MESSAGE";
        public const string ATTEMPT = "@ATTEMPT";
        public const string TIMESTAMP = "@TIMESTAMP";
    }

}
