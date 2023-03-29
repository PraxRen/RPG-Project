using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class AudioSourcesWeapon : MonoBehaviour
    {
        [SerializeField] private AudioSource _hitSound;
        [SerializeField] private Weapon _weapon;

        private void OnEnable()
        {
            _weapon.OnHited += _hitSound.Play;
        }

        private void OnDisable()
        {
            _weapon.OnHited -= _hitSound.Play;
        }
    }
}
