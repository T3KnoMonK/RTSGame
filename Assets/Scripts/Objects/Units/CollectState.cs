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

        private GameObject _CurrentResourceTarget;
        private GameObject _CurrentDepotTarget;
        private bool _IsCargoFull;
        private bool _IsPaused;

        public float GatherTime { get; private set; }

        public override void EnterState()
        {
            GatherTime = 1.0f;
            _CurrentResourceTarget = _UnitFSM.Target;
            _CurrentDepotTarget = GameObject.FindGameObjectWithTag("Depot");
            _UnitFSM.Parent.NavAgent.SetDestination(_CurrentResourceTarget.transform.position);
        }

        public override void ExitState() {
        }

        public override void UpdateState()
        {
            if(_IsPaused) return;

            _UnitFSM.CurrentPos = _UnitFSM.Parent.transform.position;

            if (_UnitFSM.CurrentPos.x == _UnitFSM.Parent.NavAgent.destination.x && _UnitFSM.CurrentPos.z == _UnitFSM.Parent.NavAgent.destination.z)
            {

                _UnitFSM.Parent.NavAgent.isStopped = true;

                if (_UnitFSM.Target == _CurrentResourceTarget)
                {
                    if (!_IsCargoFull)
                    {
                        //Enter Worker Queue on Resource, Resource calls worker Gather()
                        if (!_IsPaused)
                        {
                            EnterResourceQ();
                        }
                        //_UnitFSM.DoCoroutine(Gather());
                        _IsCargoFull = true;
                    }
                }
                else if (_UnitFSM.Target == _CurrentDepotTarget)
                {
                    if (_IsCargoFull)
                    {
                        _UnitFSM.DoCoroutine(Deposit());
                        _IsCargoFull = false;
                    }
                }
            }

            Debug.DrawLine(_UnitFSM.Parent.transform.position, _UnitFSM.Parent.NavAgent.destination, Color.blue);
        }

        public void ResumeGathering()
        {
            PauseGathering(false);
            _UnitFSM.DoCoroutine(Gather());
        }


        private void EnterResourceQ()
        {
            Resource r = _CurrentResourceTarget.GetComponent<Resource>();
            if (r.IsQFull()) { _UnitFSM.ChangeState(_UnitFSM.GetState("IDLE")); }
            r.AddToQ(_UnitFSM.Parent);
            PauseGathering(true);
        }

        public void PauseGathering(bool set)
        {
            _IsPaused = set;
            Debug.Log("Pause is set to" + set);
        }

        public IEnumerator Gather()
        {
            yield return new WaitForSeconds(GatherTime);
            _UnitFSM.Parent.Cargo += _UnitFSM.Parent.GatherRate;
            _UnitFSM.Target.SendMessage("AdjustResources", _UnitFSM.Parent.GatherRate * -1, SendMessageOptions.RequireReceiver); //Should probably try to be more consistent with the type of events that I use
            _UnitFSM.Target = _CurrentDepotTarget;
            _UnitFSM.Parent.NavAgent.SetDestination(_CurrentDepotTarget.transform.position);
            _UnitFSM.Parent.NavAgent.isStopped = false;
            _CurrentResourceTarget.GetComponent<Resource>().SendMessage("PopFromQ", SendMessageOptions.RequireReceiver);
        }

        public IEnumerator Deposit()
        {
            yield return new WaitForSeconds(GatherTime);
            DepositResourceEvent?.Invoke(_UnitFSM.Parent.GatherRate);
            _UnitFSM.Parent.Cargo = 0;
            _UnitFSM.Target = _CurrentResourceTarget;
            _UnitFSM.Parent.NavAgent.SetDestination(_CurrentResourceTarget.transform.position);
            _UnitFSM.Parent.NavAgent.isStopped = false;
        }

    }
}
