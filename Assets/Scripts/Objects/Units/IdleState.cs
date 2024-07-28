using UnityEngine;

public partial class UnitFSM
{
    //////////////STATES///////////////////////////////////////////////

    public class IdleState : State
    {
        private UnitFSM _UnitFSM;
        public IdleState(UnitFSM parent) : base(parent) { _UnitFSM = parent; }

        public override void EnterState()
        {
            _UnitFSM.Parent.NavAgent.isStopped = true;
            Debug.Log("Entered IDLE state");
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            _UnitFSM.Parent.NavAgent.isStopped = false;
        }

    }
}
