using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private bool _isHoming = true;
        [SerializeField] private GameObject _hitEffect = null;
        [SerializeField] private float _maxLifeTime = 10;
        [SerializeField] private float _lifeAfterImpact = 2;
        [SerializeField] private GameObject[] _destroyOnHit = null;

        private Health _target = null;
        private float _damage =0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            if (_isHoming == true && _target.IsDead() == false)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;

            Destroy(gameObject, _maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_target.GetComponent<Health>() != _target)
                return;

            if (_target.IsDead())
                return;

            _target.TakeDamage(_damage);
            _speed = 0;

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in _destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
