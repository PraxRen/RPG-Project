using RPG.Attributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class AudioSourcesProjectile : MonoBehaviour
    {
        [SerializeField] private AudioSource _LaunchSound;
        [SerializeField] private AudioSource _hitSound;
        [SerializeField] private Projectile _projectile;

        private void OnEnable()
        {
            _projectile.OnHit += _hitSound.Play;
        }

        private void OnDisable()
        {
            _projectile.OnHit -= _hitSound.Play;
        }
    }
}
