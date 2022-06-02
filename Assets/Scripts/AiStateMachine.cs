public class AiStateMachine
{
    public AiState[] States;
    public AiAgent Agent;
    public AiStateId CurrentState;

    public AiStateMachine(AiAgent agent)
    {
        Agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        States = new AiState[numStates];
    }

    public void RegisterState(AiState state)
    {
        int index = (int) state.GetId();
        States[index] = state;
    }

    public AiState GetState(AiStateId stateId)
    {
        int index = (int) stateId;
        return States[index];
    }

    public void Update()
    {
        GetState(CurrentState)?.Update(Agent);
    }

    public void ChangeState(AiStateId newState)
    {
        GetState(CurrentState)?.Exit(Agent);
        CurrentState = newState;
        GetState(CurrentState)?.Enter(Agent);
    }
}
