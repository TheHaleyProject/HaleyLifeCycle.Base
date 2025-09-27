using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using static Haley.Abstractions.QueryFields;

namespace Haley.Models {
    public static class QRY_WF_VERSION {
        public const string INSERT = $@"INSERT INTO WF_VERSION (WORKFLOW, VERSION, DEFINITION) VALUES ({WORKFLOW}, {VERSION}, {DEFINITION}) RETURNING guid";
        public const string SELECT_BY_ID = $@"SELECT * FROM WF_VERSION WHERE ID = {ID}";
     
        public const string SELECT_BY_GUID = $@"SELECT * FROM WF_VERSION WHERE GUID = {GUID}";
        public const string SELECT_BY_WORKFLOW_AND_VERSION = $@"SELECT * FROM WF_VERSION WHERE WORKFLOW = {WORKFLOW} AND VERSION = {VERSION}";
        public const string SELECT_LATEST = $@"SELECT * FROM WF_VERSION WHERE WORKFLOW = {WORKFLOW} ORDER BY VERSION DESC LIMIT 1";
        public const string DELETE_BY_GUID = $@"DELETE FROM WF_VERSION WHERE GUID = {GUID}";
        public const string DELETE_BY_ID = $@"DELETE FROM WF_VERSION WHERE ID = {ID}";
        public const string NEXT_VERSION_NO = $@"SELECT COALESCE(MAX(VERSION), 0) + 1 AS NEXT_VERSION FROM WF_VERSION WHERE WORKFLOW = {WORKFLOW}";
        public const string MARK_AS_PUBLISHED = $@"UPDATE WF_VERSION SET PUBLISHED = 1 WHERE GUID = {GUID}";
        public const string SELECT_LATEST_DEFINITION = $@"SELECT DEFINITION FROM WF_VERSION WHERE WORKFLOW = {WORKFLOW} ORDER BY VERSION DESC LIMIT 1";
    }
}
