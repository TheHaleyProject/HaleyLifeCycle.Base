using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Haley.Abstractions.QueryFields;

namespace Haley.Utils {
    public class MariaDBRepository : IWorkflowRepository {
        private readonly IAdapterGateway _agw;

        public MariaDBRepository(IAdapterGateway gw) {
            _agw = gw;
            if (_agw == null) throw new ArgumentNullException($@"{nameof(IAdapterGateway)} cannot be empty for this operation.");
        }

        public Task AddStepLogAsync(StepLog log) {
            throw new NotImplementedException();
        }

        public async Task<IFeedback<Guid>> CreateVersionAsync(int workflowId, string definitionJson) {
            var fb = new Feedback<Guid>();
            try {
                if (workflowId < 1 || string.IsNullOrWhiteSpace(definitionJson) || !definitionJson.IsValidJson()) return fb.SetStatus(false).SetMessage("Workflow Id should be valid. Definition Json should be a valid JSON.");

                var wfExists = await _agw.Scalar(new AdapterArgs() { Query = QRY_WORKFLOW.EXISTS_BY_ID }, (ID, workflowId));
                if (wfExists == null) return fb.SetStatus(false).SetMessage($@"No workflow exists for the given id {workflowId}");

                //First get the latest version and check if the definition is the same or not.
                try {
                    var latestDefObj = await _agw.Read(new AdapterArgs() { Query = QRY_WF_VERSION.SELECT_LATEST, Filter = ResultFilter.FirstDictionary }, (WORKFLOW, workflowId));
                    if (latestDefObj != null && latestDefObj is Dictionary<string, object> latest && latest.TryGetValue("definition", out var latestDef)) {
                        //So some latest object exists. Check if the definition is same as the one we are trying to insert.
                        var hash1 = JsonSerializer.Serialize(JsonNode.Parse(latestDef.ToString()))?.ComputeHash();
                        var hash2 = JsonSerializer.Serialize(JsonNode.Parse(definitionJson))?.ComputeHash();
                        if (hash1 == hash2) {
                            Console.WriteLine($"Hash1: {hash1}");
                            Console.WriteLine($"Hash2: {hash2}");

                            if (latest.TryGetValue("guid", out var latestGuidObj) && Guid.TryParse(latestGuidObj?.ToString(), out var latestGuid)) return fb.SetStatus(true).SetMessage("Latest Version already contains the same definition json. Not creating a new version").SetResult(latestGuid);
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine("Exception while trying to compare with latest version. Proceeding to create new version");
                }


                var versionObj = await _agw.Scalar(new AdapterArgs() { Query = QRY_WF_VERSION.NEXT_VERSION_NO }, (WORKFLOW, workflowId));
                if (versionObj == null || !int.TryParse(versionObj.ToString(), out int nextVersion)) return fb.SetStatus(false).SetMessage("Unable to calculate next version number.");

                var parameters = new Dictionary<string, object> { { WORKFLOW, workflowId }, { VERSION, nextVersion }, { DEFINITION, definitionJson } };
                Console.WriteLine($"Creating version {nextVersion} for workflow {workflowId}");

                var guidObj = await _agw.Scalar(parameters.ToAdapterArgs(QRY_WF_VERSION.INSERT));
                if (guidObj != null && Guid.TryParse(guidObj.ToString(), out Guid newGuid)) return fb.SetStatus(true).SetResult(newGuid);
                return fb.SetStatus(false).SetMessage("Failed to create workflow version.");
            } catch (Exception ex) {
                return fb.SetStatus(false).SetMessage(ex.Message);
            }
        }

        public async Task<IFeedback<Dictionary<string, object>>> CreateOrGetWorkflowAsync(int code, string name, int source =0) {
            var fb = new Feedback<Dictionary<string, object>>();
            try {
                var parameters = new Dictionary<string, object> { { SOURCE, source }, { CODE, code }, { NAME, name } };
                var fetchFunc = async () =>
                {
                    var result = await _agw.Read(new AdapterArgs { Query = QRY_WORKFLOW.SELECT_BY_CODE, Filter = ResultFilter.FirstDictionary }, (SOURCE, source), (CODE, code));
                return result is Dictionary<string, object> dic && dic.Count > 0
                        ? fb.SetStatus(true).SetResult(dic).SetMessage(string.Empty)
                        : fb.SetStatus(false).SetMessage("Workflow not found.");
                };

                var existing = await fetchFunc();
                if (existing.Status) return existing.SetMessage("Already Exists");

                if (string.IsNullOrWhiteSpace(name)) parameters[NAME] = $"WORKFLOW_{source}_{code}";
                await _agw.NonQuery(parameters.ToAdapterArgs(QRY_WORKFLOW.INSERT));

                return await fetchFunc();
            } catch (Exception ex) {
                return fb.SetStatus(false).SetMessage(ex.Message);
            }
        }

        public async Task UpdateWorkflowAsync( int code, string name, int source =0) {
            var affected = await _agw.NonQuery(
                new AdapterArgs { Query = QRY_WORKFLOW.UPDATE },
                (SOURCE, source),
                (CODE, code),
                (NAME, name)
            );
        }

        public async Task DeleteWorkflowAsync(int code) {
            await _agw.NonQuery(new AdapterArgs() { Query = QRY_WORKFLOW.DELETE }, (CODE, code));
        }


        public Task<IEnumerable<WorkflowDefinition>> LoadAllWorkflowsAsync() {
            throw new NotImplementedException();
        }

        public async Task<IFeedback<Dictionary<string, object>>> CreateOrGetAppSourceAsync(int code, string name) {
            var fb = new Feedback<Dictionary<string, object>>();
            try {
                var parameters = new Dictionary<string, object> { { CODE, code }, { NAME, name } };

                var sourceFunc = async () => {
                    var idObj = await _agw.Read(new AdapterArgs() { Query = QRY_APP_SOURCE.SELECT_BY_CODE, Filter = ResultFilter.FirstDictionary }, (CODE, code));
                    if (idObj != null && idObj is Dictionary<string, object> dic1 && dic1.Count > 0) return fb.SetStatus(true).SetMessage(string.Empty).SetResult(dic1);
                    return fb.SetStatus(false).SetMessage("Unable to create or fetch any appsource"); // Since CODE is the PK, we return it directly
                };

                var sourceCheck = await sourceFunc();
                if (sourceCheck != null && sourceCheck.Status) return sourceCheck.SetMessage("Already Exists");

                //For creating name is required.
                if (string.IsNullOrWhiteSpace(name)) parameters[NAME] = $@"APPSOURCE_{code}";
                await _agw.NonQuery(parameters.ToAdapterArgs(QRY_APP_SOURCE.INSERT));
                
                return await sourceFunc();
            } catch (Exception ex) {
                return fb.SetStatus(false).SetMessage(ex.Message);
            }
        }


        public async Task<WorkflowDefinition> LoadWorkflowByCodeAsync(int source, int code) {
            var wfObj = await _agw.Read( new AdapterArgs() { Query = QRY_WORKFLOW.SELECT_LATEST_DEFINITION , Filter = ResultFilter.FirstDictionary},
                (SOURCE, source),
                (CODE, code)
            );

            if (wfObj == null || !(wfObj is Dictionary<string, object> dic)) return null;
            return null;
        }

        public async Task<WorkflowDefinition> LoadDefinitionAsync(Guid guid) {
            return null;
        }


        public Task<WorkflowInstance> LoadInstanceAsync(Guid instanceGuid) {
            throw new NotImplementedException();
        }

        public Task<WorkflowDefinition> LoadLatestVersionAsync(int workflowId) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StepLog>> LoadLogsAsync(Guid instanceGuid) {
            throw new NotImplementedException();
        }

        public Task<WorkflowState> LoadStateAsync(Guid instanceGuid) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkflowStep>> LoadStepsAsync(Guid instanceGuid) {
            throw new NotImplementedException();
        }

        public Task<WorkflowDefinition> LoadVersionAsync(Guid versionGuid) {
            throw new NotImplementedException();
        }

        public Task<WorkflowDefinition> LoadWorkflowAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<WorkflowDefinition> LoadWorkflowByCodeAsync(int code) {
            throw new NotImplementedException();
        }

        public Task MarkVersionAsPublishedAsync(Guid versionGuid) {
            throw new NotImplementedException();
        }

        public Task SaveInstanceAsync(WorkflowInstance instance) {
            throw new NotImplementedException();
        }

        public Task UpdateInstanceAsync(WorkflowInstance instance) {
            throw new NotImplementedException();
        }

        public Task UpdateStateAsync(Guid instanceGuid, WorkflowState state) {
            throw new NotImplementedException();
        }

        public async Task<IFeedback> GetVersionAsync(int workflowId) {
            var fb = new Feedback();

            if (workflowId < 1) return fb.SetStatus(false).SetMessage("Invalid workflow ID.");

            var result = await _agw.Read(new AdapterArgs { Query = QRY_WF_VERSION.SELECT_LATEST, Filter = ResultFilter.FirstDictionary, JsonStringAsNode = true }, (WORKFLOW, workflowId));
            if (result is Dictionary<string, object> dic && dic.Count > 0)
                return fb.SetStatus(true).SetResult(dic);

            return fb.SetStatus(false).SetMessage($"No version found for workflow ID {workflowId}.");
        }


        public async Task<IFeedback> GetVersionByGUIDAsync(Guid guid) {
            var fb = new Feedback();

            if (guid == Guid.Empty) return fb.SetStatus(false).SetMessage("Invalid version GUID.");

            var result = await _agw.Read(new AdapterArgs { Query = QRY_WF_VERSION.SELECT_BY_GUID, Filter = ResultFilter.FirstDictionary, JsonStringAsNode=true }, (GUID, guid));
            if (result is Dictionary<string, object> dic && dic.Count > 0)
                return fb.SetStatus(true).SetResult(dic);

            return fb.SetStatus(false).SetMessage($"No version found for GUID {guid}.");
        }

    }
}
