using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 0.5f;

        private Vector3 _offset;

        private void Start()
        {
            _offset = _target.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position - _offset, Time.deltaTime * _speed);
        }
    }
}
