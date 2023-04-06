using RPG.Control;
using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;
        [SerializeField] private float _fadeOutTime = 0.5f;
        [SerializeField] private float _fadeInTime = 1f;
        [SerializeField] private float _fadeWaitTime = 0.5f;

        private Fader _fader;
        private SavingWrapper _savingWrapper;

        private void Start()
        {
            _fader = PersistentObjects.Instance.Fader;
            _savingWrapper = PersistentObjects.Instance.SavingWrapper;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController playerController) == true)
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            PlayerController playerController = PersistentObjects.Instance.Player.GetComponent<PlayerController>();
            playerController.enabled = false;
            yield return _fader.FadeOut(_fadeOutTime);
            _savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            _savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal, playerController);
            _savingWrapper.Save();
            yield return new WaitForSeconds(_fadeWaitTime);
            _fader.FadeIn(_fadeInTime);
            playerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal, PlayerController player)
        {
            NavMeshAgent navMeshAgent = player.GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
            player.transform.position = otherPortal._spawnPoint.position;
            player.transform.rotation = otherPortal._spawnPoint.rotation;
            navMeshAgent.enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this)
                {
                    continue;
                }

                if (portal._destination != _destination)
                {
                    continue;
                }

                return portal;
            }

            return null;
        }
    }
}
