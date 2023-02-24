using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPosints;
        
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            _healthPosints = Mathf.Max(_healthPosints - damage, 0);
            
            if (_healthPosints == 0)
            {
                Die();
            }
        }

        public object CaptureState()
        {
            return _healthPosints;
        }

        public void RestoreState(object state)
        {
            _healthPosints = (float)state;

            if (_healthPosints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}