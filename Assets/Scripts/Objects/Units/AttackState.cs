using System.Collections;
using UnityEngine;

public partial class UnitFSM
{
    public class AttackState : State
    {
        private UnitFSM _UnitFSM;
        public AttackState(UnitFSM parent) : base(parent) { _UnitFSM = parent; }

        //private float _AttackTimeBetween = 0.0f;
        private float _AttackTimer;
        private float _LookSpeed = 2.0f;
        private Transform _UnitTransform;

        private bool _CanAttack;
        //private bool _IsFacingTarget;

        IEnumerator AttackCycle()
        {
            _CanAttack = false;
            yield return new WaitForSeconds(_AttackTimer);
            _CanAttack = true;
        }

        public override void EnterState()
        {
            _UnitTransform = _UnitFSM.Parent.transform;
            _AttackTimer = _UnitFSM.Parent.AttackSpeed;
            _CanAttack = true;
        }

        private void Attack()
        {
            //TODO: Attack Animation
            if (_UnitFSM.Parent.BulletParticle != null) { _UnitFSM.Parent.BulletParticle.Play(); }
            _UnitFSM.Target.SendMessage("TakeDamage", _UnitFSM.Parent.Damage, SendMessageOptions.DontRequireReceiver);
            _UnitFSM.Parent.StartCoroutine(AttackCycle());
        }

        private float DotProductToTarget()
        {
            Vector3 bmina = (_UnitFSM.Target.transform.position - _UnitFSM.Parent.transform.position).normalized;
            float dot = Vector3.Dot(bmina, _UnitFSM.Parent.transform.forward);
            if(1.0f - dot <= 0.05f) { dot = 1.0f; }
            return dot;
        }

        public override void UpdateState()
        {
            //Debug.Log("Can attack: " + _CanAttack.ToString());
            if(_UnitFSM.Target != null)
            {
                if (WithinAttackDistance())
                {
                    _UnitFSM.Parent.NavAgent.isStopped = true; 

                    //Debug.Log("Facing target: " + FacingTarget().ToString());
                    if (FacingTarget())
                    {
                        if (_CanAttack)
                        {
                            Attack();
                        }
                    }
                    else
                    {
                        RotateToTarget();
                    }
                }
                else
                {
                    MoveToTarget();
                }
            }
            else
            {
                _UnitFSM.ChangeState(_UnitFSM.GetState("IDLE"));
            }

            Debug.DrawLine(_UnitFSM.Parent.transform.position, _UnitFSM.Parent.NavAgent.destination, Color.red);

        }

        private float DistanceToTarget() {
            return (_UnitFSM.Target.transform.position - _UnitTransform.position).magnitude;
        }

        private bool WithinAttackDistance()
        {
            return DistanceToTarget() <= _UnitFSM.Parent.AttackDistance;
        }

        private bool FacingTarget() {
            return Mathf.Approximately(DotProductToTarget(), 1.0f);
        }

        private void RotateToTarget() {
            Quaternion lookTarget = Quaternion.LookRotation(_UnitFSM.Target.transform.position - _UnitFSM.Parent.transform.position);
            _UnitTransform.rotation = Quaternion.Slerp(_UnitTransform.rotation, lookTarget, _LookSpeed * Time.deltaTime);
        }

        private void MoveToTarget()
        {
            _UnitFSM.Parent.NavAgent.isStopped = false;
            _UnitFSM.Parent.NavAgent.SetDestination(_UnitFSM.Target.transform.position);
        }

        public override void ExitState(){}

    }
}
