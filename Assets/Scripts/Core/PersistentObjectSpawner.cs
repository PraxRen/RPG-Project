using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;

        private static bool _hasSpawned = false;

        private void Awake()
        {
            if (_hasSpawned == true)
            {
                return;
            }

            Instantiate(persistentObjectPrefab);
            _hasSpawned = true;
        }
    }
}