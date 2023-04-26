using RPG.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Health), typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private float _suspicionTime = 5f;
        [SerializeField] private float _aggroCooldownTime = 5f;
        [SerializeField] private PatrolPath _patrolPath;
        [SerializeField] private float _waypointTolerance = 1f;
        [SerializeField] private float _waypointDwellTime = 3f;
        [Range(0f, 1f)]
        [SerializeField] private float _patrolSpeedFraction = 0.2f;
        [SerializeField] private float _shoutDistance = 5f;

        private Health _health;
        private Fighter _fighter;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private GameObject _player;
        private LazyValue<Vector3> _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float _timeSinceAggrevated = Mathf.Infinity;
        private int _currentWaypointIndex = 0;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void OnEnable()
        {
            _health.OnTakeDamage += Aggrevate;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= Aggrevate;
        }

        private void Start ()
        {
            _player = PersistentObjects.Instance.Player;
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if (_health.IsDead == true)
                return;

            UpdateTimers();
            bool isAggrevated = IsAggrevated();
            bool isCanAttack = _fighter.CanAttack(_player);

            if (isAggrevated && isCanAttack)
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }

        public void Aggrevate()
        {
            _timeSinceAggrevated = 0;
        }

        public void Aggrevate(float damage)
        {
            Aggrevate();
        }
        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition.value;

            if (_patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime) 
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _timeSinceArrivedAtWaypoint = 0;
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0f;
            _fighter.Attack(_player);
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _shoutDistance,Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();

                if (ai == null)
                    continue;

                if (ai == this)
                    continue;

                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            float distanceToplayer = Vector3.Distance(_player.transform.position, transform.position);
            bool isPlayerPositionNearby = distanceToplayer < _chaseDistance;
            bool isTimeAggrivated = _timeSinceAggrevated < _aggroCooldownTime;
            return isPlayerPositionNearby || isTimeAggrivated;
        }
    }
}