using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class AudioSourcesCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _damageSound;
        [SerializeField] private AudioSource _dieSound;
        [SerializeField] private Health _health;

        private void OnEnable()
        {
            _health.OnTakeDamage += TakeDamage;
            _health.OnDie += _dieSound.Play;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= TakeDamage;
            _health.OnDie -= _dieSound.Play;
        }

        private void TakeDamage(float damageAmount)
        {
            _damageSound.Play();
        }
    }
}
