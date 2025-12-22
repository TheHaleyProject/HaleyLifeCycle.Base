using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public partial class QueryFields {
        // 🔹 Common Identity Fields
        public const string ID = "@ID";
        public const string GUID = "@GUID";
        public const string NAME = "@NAME";
        public const string CODE = "@CODE";

        // 🔹 WORKFLOW
        public const string SOURCE = "@SOURCE";

        // 🔹 WF_VERSION
        public const string WORKFLOW = "@WORKFLOW";         // FK → WORKFLOW.ID
        public const string VERSION = "@VERSION";           // integer version number
        public const string DEFINITION = "@DEFINITION";     // JSON/text definition

        // 🔹 ENVIRONMENT & ENGINE
        public const string ENVIRONMENT = "@ENVIRONMENT";   // FK → ENVIRONMENT.CODE
        public const string ENGINE = "@ENGINE";             // FK → WF_ENGINE.ID
        public const string STATUS = "@STATUS";             // Engine or instance status
        public const string LAST_BEAT = "@LAST_BEAT";       // Timestamp for heartbeat
        public const string ENGINE_GUID = "@ENGINE_GUID";   // Unique engine identifier
        public const string ENGINE_LASTBEAT = "last_beat";

        // 🔹 WF_INSTANCE
        public const string WF_VERSION = "@WF_VERSION";     // FK → WF_VERSION.ID
        public const string LOCKED_BY = "@LOCKED_BY";       // FK → WF_ENGINE.ID
        public const string CREATED = "@CREATED";           // Timestamp
        public const string MODIFIED = "@MODIFIED";         // Timestamp

        // 🔹 WFI_BELONGS_TO
        public const string WFI_ID = "@WFI_ID";             // FK → WF_INSTANCE.ID
        public const string SOURCE_APP = "@SOURCE";         // FK → APP_SOURCE.CODE
        public const string OWNER = "@OWNER";               // Optional owner ID
        public const string REF = "@REF";                   // External reference

        // 🔹 WF_INFO
        public const string WFI = "@WFI";                   // FK → WF_INSTANCE.ID
        public const string PARAMETERS = "@PARAMETERS";     // JSON startup parameters
        public const string URL_OVERRIDES = "@URL_OVERRIDES"; // JSON endpoint overrides

        // 🔹 Claiming & Filtering
        public const string STATUS_LIST = "@STATUS_LIST";   // Comma-separated status codes
        public const string LIMIT = "@LIMIT";               // Row limit for claim

  
    }

}
