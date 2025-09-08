using Haley.Abstractions;
using Haley.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Utils {
    public class MariaDBRepository : IWorkflowRepository {
        private readonly IAdapterGateway _agw;

        public MariaDBRepository(IAdapterGateway gw) {
            _agw = gw;
        }

        public async Task<WorkflowDefinition> LoadDefinitionAsync(Guid definitionId) {
            var sql = "SELECT * FROM wf_info WHERE id = @Id";
            var definition = await _agw.QuerySingleOrDefaultAsync<WorkflowDefinition>(sql, new { Id = definitionId });

            if (definition != null) {
                definition.Phases = (await _agw.QueryAsync<WorkflowPhase>(
                    "SELECT * FROM wf_phase WHERE workflow_id = @Id", new { Id = definitionId })).ToList();

                definition.Steps = (await _agw.QueryAsync<WorkflowStep>(
                    "SELECT * FROM step_info WHERE workflow_id = @Id", new { Id = definitionId })).ToList();
            }

            return definition;
        }

        public async Task SaveInstanceAsync(WorkflowInstance instance) {
            var sql = @"INSERT INTO wf_instance (id, definition_id, reference, state, created_at, updated_at, environment)
                    VALUES (@InstanceId, @DefinitionId, @Reference, @State, @CreatedAt, @LastUpdated, @Environment)";
            await _agw.ExecuteAsync(sql, instance);
        }

        public async Task<WorkflowInstance> LoadInstanceAsync(Guid instanceId) {
            var sql = "SELECT * FROM wf_instance WHERE id = @Id";
            var instance = await _agw.QuerySingleOrDefaultAsync<WorkflowInstance>(sql, new { Id = instanceId });

            return instance;
        }

        public async Task UpdateInstanceAsync(WorkflowInstance instance) {
            var sql = @"UPDATE wf_instance SET state = @State, updated_at = @LastUpdated, is_suspended = @IsSuspended
                    WHERE id = @InstanceId";
            await _agw.ExecuteAsync(sql, instance);
        }

        public async Task<WorkflowState> LoadStateAsync(Guid instanceId) {
            var sql = "SELECT * FROM wfi_state WHERE instance_id = @Id";
            var state = await _agw.QuerySingleOrDefaultAsync<WorkflowState>(sql, new { Id = instanceId });

            if (state != null) {
                var results = await _agw.QueryAsync<StepResult>(
                    "SELECT * FROM wfi_step WHERE instance_id = @Id", new { Id = instanceId });
                state.StepResults = results.ToDictionary(r => r.StepCode, r => r);

                var logs = await _agw.QueryAsync<StepLog>(
                    "SELECT * FROM attempt_log WHERE instance_id = @Id", new { Id = instanceId });
                state.Logs = logs.ToList();
            }

            return state;
        }

        public async Task UpdateStateAsync(Guid instanceId, WorkflowState state) {
            var sql = @"UPDATE wfi_state SET status = @Status, current_phase_code = @CurrentPhaseCode,
                    current_step_code = @CurrentStepCode WHERE instance_id = @InstanceId";
            await _agw.ExecuteAsync(sql, new {
                InstanceId = instanceId,
                Status = state.Status,
                CurrentPhaseCode = state.CurrentPhaseCode,
                CurrentStepCode = state.CurrentStepCode
            });

            foreach (var result in state.StepResults.Values) {
                await _agw.ExecuteAsync("REPLACE INTO wfi_step (...) VALUES (...)", result); // Fill in columns
            }

            foreach (var log in state.Logs) {
                await _agw.ExecuteAsync("INSERT INTO attempt_log (...) VALUES (...)", log); // Fill in columns
            }
        }
    }

}
