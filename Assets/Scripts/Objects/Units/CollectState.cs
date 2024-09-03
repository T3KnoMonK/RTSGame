using System.Collections;
using UnityEngine;

public partial class UnitFSM
{
    public class CollectState : State
    {
        public delegate void DepositResourceDelegate(int amount);
        public static DepositResourceDelegate DepositResourceEvent;

        private UnitFSM _UnitFSM;
        public CollectState(UnitFSM parent) : base(parent) { _UnitFSM = parent; }

        private GameObject currentResourceTarget;
        private GameObject currentDepotTarget;
        private bool isCargoFull;

        public float GatherTime { get; private set; }

        public override void EnterState()
        {
            GatherTime = 1.0f;
            currentResourceTarget = _UnitFSM.Target;
            currentDepotTarget = GameObject.FindGameObjectWithTag("Depot");
            _UnitFSM.Parent.NavAgent.SetDestination(currentResourceTarget.transform.position);
        }

        public override void ExitState() {
        }

        public override void UpdateState()
        {
            _UnitFSM.CurrentPos = _UnitFSM.Parent.transform.position;

            if (_UnitFSM.CurrentPos.x == _UnitFSM.Parent.NavAgent.destination.x && _UnitFSM.CurrentPos.z == _UnitFSM.Parent.NavAgent.destination.z)
            {

                _UnitFSM.Parent.NavAgent.isStopped = true;

                if (_UnitFSM.Target == currentResourceTarget)
                {
                    if (!isCargoFull)
                    {
                        //Enter Worker Queue on Resource, Resource calls worker Gather()
                        isCargoFull = true;
                        _UnitFSM.DoCoroutine(Gather());
                    }
                }
                else if (_UnitFSM.Target == currentDepotTarget)
                {
                    if (isCargoFull)
                    {
                        isCargoFull = false;
                        _UnitFSM.DoCoroutine(Deposit());
                    }
                }
            }

            Debug.DrawLine(_UnitFSM.Parent.transform.position, _UnitFSM.Parent.NavAgent.destination, Color.blue);
        }

        public IEnumerator Gather()
        {
            yield return new WaitForSeconds(GatherTime);
            _UnitFSM.Parent.Cargo += _UnitFSM.Parent.GatherRate;
            _UnitFSM.Target.SendMessage("AdjustResources", _UnitFSM.Parent.GatherRate * -1, SendMessageOptions.RequireReceiver); //Should probably try to be more consistent with the type of events that I use
            _UnitFSM.Target = currentDepotTarget;
            _UnitFSM.Parent.NavAgent.SetDestination(currentDepotTarget.transform.position);
            _UnitFSM.Parent.NavAgent.isStopped = false;
        }

        public IEnumerator Deposit()
        {
            yield return new WaitForSeconds(GatherTime);
            DepositResourceEvent?.Invoke(_UnitFSM.Parent.GatherRate);
            _UnitFSM.Parent.Cargo = 0;
            _UnitFSM.Target = currentResourceTarget;
            _UnitFSM.Parent.NavAgent.SetDestination(currentResourceTarget.transform.position);
            _UnitFSM.Parent.NavAgent.isStopped = false;
        }

    }
}
