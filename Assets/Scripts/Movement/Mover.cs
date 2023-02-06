using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float _maxSpeed = 6f;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Health _health;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped= true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat("forwardSpeed", speed);
        }


    }
}
