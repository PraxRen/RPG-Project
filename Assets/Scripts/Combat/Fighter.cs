using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttack = 1f;
        [SerializeField] private float _weaponDamage = 5f;

        private Health _target;
        private Mover _mover;
        float _timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null)
                return;

            if (_target.IsDead())
                return;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if(_timeSinceLastAttack > _timeBetweenAttack)
            {
                //This will trigger the Hit() event
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public void Hit()
        {
            _target?.TakeDamage(_weaponDamage);
        }

 
    }
} 