using RPG.Attributes;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;
        [SerializeField] private Weapon _equippedPrefab = null;
        [SerializeField] private float _weaponDamage = 5f;
        [SerializeField] private float _percentageBonus = 0f;
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile = null;

        private const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;

            if (_equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(_equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
            else if (overrideController != null)
            {
                
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null)
                return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(_projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage() 
        {
            return _weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return _percentageBonus;
        }

        public float GetRange()
        {
            return _weaponRange;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransfoem;

            if (_isRightHanded)
                handTransfoem = rightHand;
            else
                handTransfoem = leftHand;

            return handTransfoem;
        }
    }
}
