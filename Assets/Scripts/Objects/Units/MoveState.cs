using UnityEngine;
using WebSocketSharp;

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

            Vector2 to = new Vector2(_UnitFSM.Parent.NavAgent.destination.x, _UnitFSM.Parent.NavAgent.destination.z);
            Vector2 from = new Vector2(_UnitFSM.CurrentPos.x, _UnitFSM.CurrentPos.z);

            float dist = (to - from).magnitude;
            float offset = (_UnitFSM.Parent.GetComponent<BoxCollider>().bounds.extents.x * _UnitFSM.Parent.GetComponent<BoxCollider>().bounds.extents.z) / 2; //average the extents for simplicity for now.

            if (dist <= offset)
            {
                if(_UnitFSM.ManualMoveAction == true)
                {
                    if (_UnitFSM.parentSO.unitType == 0)
                    {
                        _UnitFSM.Parent.WorkerHasArrived();
                    }
                }
                _UnitFSM.ChangeState(_UnitFSM.GetState("IDLE"));
            }

            Debug.DrawLine(_UnitFSM.Parent.transform.position, _UnitFSM.Parent.NavAgent.destination, Color.green);
        }

    }
}
