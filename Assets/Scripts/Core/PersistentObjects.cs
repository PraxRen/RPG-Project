using RPG.SceneManagement;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjects : MonoBehaviour
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Fader _fader;
        [SerializeField] SavingWrapper _savingWrapper;

        public static PersistentObjects Instance { get; private set; }
        public GameObject Player => _playerPrefab;
        public Fader Fader => _fader;
        public SavingWrapper SavingWrapper => _savingWrapper;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _playerPrefab = Instantiate(_playerPrefab);
            DontDestroyOnLoad(Player);
            DontDestroyOnLoad(gameObject);
        }
    }
}