using RPG.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(Animator), typeof(BaseStats))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _timeBetweenAttack = 1f;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private WeaponConfig _defaultWeaponConfig = null;
        [SerializeField] private Equipment _equipment;

        private Health _target;
        private Mover _mover;
        private Animator _animator;
        private BaseStats _baseStats;
        private ActionScheduler _actionScheduler;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private WeaponConfig _currentWeaponConfig;
        private LazyValue<Weapon> _currentWeapon;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _currentWeaponConfig = _defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void OnEnable()
        {
            _equipment.equipmentUpdated += UpdateWeapon;
        }

        private void OnDisable()
        {
            _equipment.equipmentUpdated += UpdateWeapon;
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null)
                return;

            if (_target.IsDead)
                return;

            if (!GetIsInRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeaponConfig = weaponConfig;
            _currentWeapon.value = AttachWeapon(weaponConfig);
        }

        public Health GetTarget()
        {
            return _target;
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            if (!_mover.CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform))
                return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            WeaponConfig weaponConfig = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;

            if (weaponConfig == null)
            {
                EquipWeapon(_defaultWeaponConfig);
                return;
            }
            
            EquipWeapon(weaponConfig);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(_defaultWeaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(_rightHandTransform, _leftHandTransform, _animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if(_timeSinceLastAttack > _timeBetweenAttack)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _currentWeaponConfig.GetRange();
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(AnimatorCharacterManager.Instance.Params.StopAttack);
            _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Attack);
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(AnimatorCharacterManager.Instance.Params.Attack);
            _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.StopAttack);
        }

        private void Hit()
        {
            if (_target == null)
                return;

            float damage = _baseStats.GetStat(Stat.Damage);

            if (_currentWeapon.value != null)
            {
                _currentWeapon.value.OnHit();
            }

            if (_currentWeaponConfig.HasProjectile())
            {
                _currentWeaponConfig.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }
    }
} 