public enum AiStateId
{
    Seek,
    Attack,
    Flee
}


public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
    
}
