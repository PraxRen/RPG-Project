using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText _damageTextPrefab = null;
        [SerializeField] Health _health = null;

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(_damageTextPrefab, transform); 
            instance.SetValue(damageAmount);
        }

        private void OnEnable()
        {
            _health.OnTakeDamage += Spawn;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= Spawn;
        }
    }
}

