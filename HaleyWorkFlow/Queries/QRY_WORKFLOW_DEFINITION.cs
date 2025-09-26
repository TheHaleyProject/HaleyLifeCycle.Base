using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using static Haley.Abstractions.QueryFields;

namespace Haley.Models {
    public static class QRY_WF_DEFINITION {
        public const string INSERT = $@"
        INSERT INTO wf_definition (def_id, version, name, description, created_at, created_by)
        VALUES ({@DEF_ID}, {@VERSION}, {@NAME}, {@DESCRIPTION}, {@CREATED_AT}, {@CREATED_BY})";

        public const string SELECT_BY_ID = $@"
        SELECT * FROM wf_definition
        WHERE def_id = {@DEF_ID} AND version = {@VERSION}";

        public const string SELECT_LATEST = $@"
        SELECT * FROM wf_definition
        WHERE def_id = {@DEF_ID}
        ORDER BY version DESC LIMIT 1";

        public const string DELETE = $@"
        DELETE FROM wf_definition
        WHERE def_id = {@DEF_ID} AND version = {@VERSION}";
    }

}
