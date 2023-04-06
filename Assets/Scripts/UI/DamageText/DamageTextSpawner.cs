using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText _damageTextPrefab = null;
        [SerializeField] Health _health = null;

        private void OnEnable()
        {
            _health.OnTakeDamage += Spawn;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= Spawn;
        }

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(_damageTextPrefab, transform); 
            instance.SetValue(damageAmount);
        }
    }
}

