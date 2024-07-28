using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public Dictionary<string, State> states = new Dictionary<string, State>();
    public State defaultState;
    public State currentState;

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        if (currentState != null) { currentState.UpdateState(); }
    }

    public virtual void ChangeState(State newState)
    {
        //Debug.Log("Exiting state: " + currentState.ToString());
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
        //Debug.Log("Entered state: " + currentState.ToString());
    }

}
