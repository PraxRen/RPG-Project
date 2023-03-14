using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float _regenerationPercentage = 70f;
        
        public UnityEvent<float> OnTakeDamage;
        public UnityEvent OnDie;


        private LazyValue<float> _healthPoints;
        private bool _isDead = false;

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            
            if (_healthPoints.value == 0)
            {
                OnDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                OnTakeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;

            if (_healthPoints.value == 0)
            {
                Die();
            }
        }

        public void Heal(float healthToRestore)
        {
            _healthPoints.value = Mathf.Min(_healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        private void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null)
                return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthpoints = GetComponent<BaseStats>().GetStat(Stat.Health) * _regenerationPercentage / 100;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthpoints);
        }
    }
}                     