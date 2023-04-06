using RPG.Control;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool _alreadyTriggered = false;
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.gameObject.TryGetComponent(out PlayerController playerController) == true)
            {
                _alreadyTriggered = true;
                _playableDirector.Play();
            }
        }

        public object CaptureState()
        {
            return _alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            _alreadyTriggered = (bool)state;
        }
    }
}