using RPG.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats), typeof(Animator), typeof(ActionScheduler))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _regenerationPercentage = 70f;

        private BaseStats _baseStats;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private LazyValue<float> _healthPoints;
        private bool _isDead = false;

        public bool IsDead => _isDead;

        public event Action<float> OnTakeDamage;
        public event Action OnDie;

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
            _baseStats = GetComponent<BaseStats>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void OnEnable()
        {
            _baseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= RegenerateHealth;
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            
            if (_healthPoints.value == 0)
            {
                OnDie?.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                OnTakeDamage?.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return _healthPoints.value / _baseStats.GetStat(Stat.Health);
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

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            _animator.SetTrigger(AnimatorCharacterManager.Instance.Params.Die);
            _actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null)
                return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthpoints = _baseStats.GetStat(Stat.Health) * _regenerationPercentage / 100;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthpoints);
        }
    }
}                     