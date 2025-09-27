using Haley.Enums;
using Haley.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions {
    public interface IWorkflowEngine {
        Task<Guid> StartWorkflowAsync(Guid definitionId, Dictionary<string, object> parameters, Dictionary<string, string> urlOverrides);
        Task ExecuteAsync(Guid instanceId);
        //Task<WorkflowDefinition> LoadDefinitionAsync(Guid definitionId);
        //Task<StepResult> ExecuteStepAsync(WorkflowStep step, Dictionary<string, object> parameters, Dictionary<string, string> urlOverrides);
        //Task MonitorTimeoutAsync(WorkflowStep step, WorkflowInstance instance, WorkflowState state);
        Task HandleWebhookAsync(Guid instanceId, string eventKey, Dictionary<string, object> payload);
    }
}
