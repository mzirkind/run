using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine StateMachine;
    public AiStateId initialState;
    public AiConfig config;
    void Start()
    {
        StateMachine = new AiStateMachine(this);
        StateMachine.ChangeState(initialState);
    }
    
    void Update()
    {
        StateMachine.Update();
    }
}
