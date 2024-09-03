using UnityEngine;

public partial class UnitFSM
{
    public class MoveState : State
    {
        private UnitFSM _UnitFSM;
        public MoveState(UnitFSM parent) : base(parent) { _UnitFSM = parent; }

        public override void EnterState()
        {
            if(_UnitFSM.Parent.NavAgent.isStopped == true)
            {
                _UnitFSM.Parent.NavAgent.isStopped = false;
            }
            _UnitFSM.Parent.NavAgent.SetDestination(_UnitFSM.ClickPos);
        }

        public override void ExitState() {}

        public override void UpdateState()
        {
            _UnitFSM.CurrentPos = _UnitFSM.Parent.transform.position;

            if (_UnitFSM.CurrentPos.x == _UnitFSM.Parent.NavAgent.destination.x && _UnitFSM.CurrentPos.z == _UnitFSM.Parent.NavAgent.destination.z)
            {
                if(_UnitFSM.ManualMoveAction == true)
                {
                    //At this time this block is only used to determine if the worker has arrived at the location of a building the player is placing
                    if (_UnitFSM.parentSO.unitType == 0)
                    {
                        Player.Instance.SendMessage("WorkerHasArrived", SendMessageOptions.DontRequireReceiver);
                    }
                }

                _UnitFSM.ChangeState(_UnitFSM.GetState("IDLE"));
            }

            Debug.DrawLine(_UnitFSM.Parent.transform.position, _UnitFSM.Parent.NavAgent.destination, Color.green);
        }

    }
}
