using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _health = 100f;
        
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            
            if (_health == 0)
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
        }
    }
}