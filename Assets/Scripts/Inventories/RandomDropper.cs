using RPG.Attributes;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    [RequireComponent(typeof(Health))]
    public class RandomDropper : ItemDropper
    {
        [SerializeField] private float _scatterDistance = 1;
        [SerializeField] private DropLibrary _dropLibrary;

        private const int Attempts = 30;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnDie += RandomDrop;
        }

        private void OnDisable()
        {
            _health.OnDie -= RandomDrop;
        }

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();

            var drops = _dropLibrary.GetRandomDrops(baseStats.GetLevel());

            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < Attempts; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * _scatterDistance;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }

            }

            return transform.position;
        }
    }
}