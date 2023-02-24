using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool _alreadyTriggered = false;

        public object CaptureState()
        {
            return _alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            _alreadyTriggered = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.gameObject.tag == "Player")
            {
                _alreadyTriggered = true; 
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}