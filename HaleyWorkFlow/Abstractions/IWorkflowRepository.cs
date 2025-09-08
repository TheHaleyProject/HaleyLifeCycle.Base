using Haley.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public interface IWorkflowRepository {
        Task<WorkflowDefinition> LoadDefinitionAsync(Guid definitionId);
        Task SaveInstanceAsync(WorkflowInstance instance);
        Task<WorkflowInstance> LoadInstanceAsync(Guid instanceId);
        Task UpdateInstanceAsync(WorkflowInstance instance);
        Task<WorkflowState> LoadStateAsync(Guid instanceId); // <-- Add this
        Task UpdateStateAsync(Guid instanceId, WorkflowState state);
    }

}
