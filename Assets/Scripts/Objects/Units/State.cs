public class State
{
    public FSM Parent;
    public State(FSM parent) { Parent = parent; }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }

}
