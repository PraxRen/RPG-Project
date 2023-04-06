using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(SavingSystem))]
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private float _fadeInTime = 0.2f;

        private const string defaultSaveFile = "save";
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            _savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            _savingSystem.Save(defaultSaveFile);
        }

        public void Delete()
        {
            _savingSystem.Delete(defaultSaveFile);
        }

        private IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(defaultSaveFile);
            _fader.FadeOutImmediate();
            yield return _fader.FadeIn(_fadeInTime);
        }
    }
}