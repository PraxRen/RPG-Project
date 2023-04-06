using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCameraHelper : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        private GameObject _player;

        private void Start()
        {
            _player = PersistentObjects.Instance.Player;
            _camera.Follow = _player.transform;
        }
    }
}