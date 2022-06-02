using UnityEngine;

public class SeekState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Seek;
    }

    public void Enter(AiAgent agent)
    {
        
    }

    public void Update(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
