#H1 Haley Lifecycle.

State Definition : Describes What COULD happen. iow, logical states, transitions.
State Instance : Describes What DID happen.

Hub (Orchestrator) Definition : Describes what should we do when it happens.. iow, How each step is executed.

## H2 GENERAL WORKFLOW

1.  Hub defines: “When event Approve happens → call ApproveAPI → move to Approved”
2.  Agent receives job → executes ApproveAPI.
3.  On success → Agent calls State API:
       POST /state/transition
       { instanceId, from: "UnderReview", to: "Approved", event: "Approve" }
4.  State validates transition (per its JSON definition).
5.  State persists:
       - new current_state = Approved
       - transition log record
6.  Agent notifies Hub: “Transition successful.”
7.  Hub updates workflow instance status accordingly.

So:

Agent performs the work.
State records what happened.
Hub observes and coordinates.

## H2 Role : AGENT VS STATE

# AGENT: 
    The Agent is the executor — it takes workflow logic from the Hub, performs actions, and records the results in the State machine.

# STATE MACHINE:
    STATE MACHINE IS A PURE LIFECYCLE LEDGER. STATE MACHINE SHOULD NOT KNOW ABOUT THE STEP PROGRESS.. THAT IS AGENT AND HUB RESPONSIBILITY. STATE MACHINE SHOULD NOT BE POLLUTED. IT SHOULD ONLY CONTAIN FINALIZED TRANSITIONS.


Agent:

1. reads the Workflow Definition (from Hub)
2. understands the current State (from State DB)
3. executes the steps/actions
4. tells the State machine “I’ve moved from X → Y”

Remember that the state applies to the entire (workflow instance) while Steps are operational , they occur with in a state.

Example : State = Macro Lifecycle, Step = Micro Worfklow Operation

Workflow Instance (1)
└── State Machine controls overall phase
    ├── State: Initiated
    │   ├── Step 1: Validate
    │   ├── Step 2: Dispatch
    ├── State: AwaitingApproval
    │   ├── Step 3: NotifyApprover
    │   ├── Step 4: WaitForResponse
    └── State: Approved
        ├── Step 5: Publish

## H3 RESPONSIBILITY CONTRACT

Concern: What is the overall workflow status?
Belongs to : State Machine
Why : It owns sm_instance.current_state and sm_transition_log, which represent the authoritative lifecycle record.

Concern: What is the current step being executed?
Belongs to : Agent (persisted via Hub)
Why : This is runtime-specific execution context (micro progress).

Concern: How are steps defined and orchestrated?
Belongs to : Workflow Hub
Why : Hub defines structure, dependencies, retries, and I/O rules.

Concern: What happens if an Agent dies mid-step?
Belongs to : Agent / Hub
Why : Hub detects abandoned instances and reassigns work; Agent on startup queries for orphaned executions.

Concern: Where is step execution progress persisted?
Belongs to : Hub DB (runtime tables)
Why : Prevents State DB pollution and centralizes telemetry and recovery data.

Concern: Who triggers state transitions?
Belongs to : Agent
Why : Transitions are committed only after step groups succeed.

Concern: Who validates state transitions?
Belongs to : State Machine
Why : Ensures that transitions follow the allowed definitions.

Concern: Who handles retries and timeouts?
Belongs to : Agent (guided by Hub policy)
Why : Retry logic is an execution concern, not a state concern.

## H3 State machine responsibilities

Feature: State validation
Included: Yes
Justification: Validates legal transitions as per state definition.

Feature: Transition history
Included: Yes
Justification: Maintains an audit trail of every state change.

Feature: Step progress
Included: No
Justification: Micro-level progress belongs to Agent and Hub.

Feature: Retry counters
Included: No
Justification: Retries are execution logic handled by Agent.

Feature: Heartbeat / ownership
Included: No
Justification: Agent and Hub manage agent liveness and ownership.

Feature: Human approval waiting
Included: No
Justification: Represented as a state (e.g., AwaitingApproval) but the actual waiting mechanism is Hub’s responsibility.

Feature: External notifications
Included: No
Justification: All side effects and communication belong to Agent execution, not State tracking.

## h2 Workflow Execution Hierarchy

Workflow Instance (macro process)
│
├── State Machine (macro lifecycle)
│     ├── State: InitialDispatch
│     ├── State: ResponseHandling
│     ├── State: DocumentValidation
│     └── State: Completed / Failed
│
└── Workflow Engine (micro execution)
      ├── Phase 1: InitialDispatch (linked to State: InitialDispatch)
      │    ├── Step 1: FetchUserProfile
      │    ├── Step 2: SendSurveyEmail
      │    └── Step 3: WaitForResponse
      │
      ├── Phase 2: ResponseHandling (linked to State: ResponseHandling)
      │    ├── Step 4: EvaluateResponse
      │    ├── Step 5: LogPositiveResponse / LogNegativeResponse
      │    └── Step 6: TriggerFollowUp
      │
      ├── Phase 3: DocumentValidation (linked to State: DocumentValidation)
      │    ├── Step 7: ValidateDoc-A
      │    ├── Step 8: ValidateDoc-B
      │    └── Step 9: ArchiveSurvey
      │
      └── Final: Completed / Failed (terminal states)
