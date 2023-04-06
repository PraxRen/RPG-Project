using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        private PlayerController _playerController;
        private ActionScheduler _playerActionScheduler;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _playerController = PersistentObjects.Instance.Player.GetComponent<PlayerController>();
            _playerActionScheduler = PersistentObjects.Instance.Player.GetComponent<ActionScheduler>();
        }

        private void OnEnable()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {

            _playerActionScheduler.CancelCurrentAction();
            _playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            _playerController.enabled = true;
        }
    }
}