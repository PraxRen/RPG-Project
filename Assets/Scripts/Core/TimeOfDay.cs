using RPG.Saving;
using System;
using System.Linq;
using UnityEngine;

namespace RPG.Core
{
    public class TimeOfDay : MonoBehaviour, ISaveable
    {
        public enum TypeTimeOfDay
        {
            Morning,
            Day,
            Evening,
            Night
        }

        [Serializable]
        public struct SkyboxSettings
        {
            public TypeTimeOfDay TypeTimeOfDay;
            public Material Skybox;
            public float TimeOfDay;
        }

        [SerializeField] private Light _sun;
        [SerializeField] private float _secondsInFullDay = 120f;
        [Range(0, 1)][SerializeField] private float _currentTimeOfDay = 0;
        [SerializeField] private float _timeMultiplier = 1f;
        [SerializeField] private SkyboxSettings[] _skyboxes = null;

        private const float _sunRotationOffset = 360f; //смещение поворота солнца
        private const float _sunElevationAngle = 90f; //угол высоты солнца над горизонтом
        private const float _sunAzimuthAngle = 170f; //азимутный угол солнца относительно меридиана

        public float SecondsInFullDay => _secondsInFullDay;
        public int CountSkybox => _skyboxes.Length;

        public event Action OnMorning;
        public event Action OnDay;
        public event Action OnEvening;
        public event Action OnNight;

        private void Start()
        {
            Array.Sort(_skyboxes, (x, y) => x.TimeOfDay.CompareTo(y.TimeOfDay));
        }

        private void Update()
        {
            UpdateSun();
            _currentTimeOfDay += (Time.deltaTime / _secondsInFullDay) * _timeMultiplier;

            if (_currentTimeOfDay >= 1)
                _currentTimeOfDay = 0;
        }

        public object CaptureState()
        {
            return _currentTimeOfDay;
        }

        public void RestoreState(object state)
        {
            _currentTimeOfDay = (float)state;
        }

        private void UpdateSun()
        {
            float sunAngle = (_currentTimeOfDay * _sunRotationOffset) - _sunElevationAngle;
            _sun.transform.localRotation = Quaternion.Euler(sunAngle, _sunAzimuthAngle, 0f);

            UpdateSkybox();
        }

        private void UpdateSkybox()
        {
            SkyboxSettings skyboxNext = _skyboxes.FirstOrDefault(skybox => _currentTimeOfDay < skybox.TimeOfDay);

            if (RenderSettings.skybox == skyboxNext.Skybox)
            {
                return;
            }

            RenderSettings.skybox = skyboxNext.Skybox;

            switch (skyboxNext.TypeTimeOfDay)
            {
                case TypeTimeOfDay.Morning:
                    OnMorning?.Invoke();
                    break;
                case TypeTimeOfDay.Day:
                    OnDay?.Invoke();
                    break;
                case TypeTimeOfDay.Evening:
                    OnEvening?.Invoke();
                    break;
                case TypeTimeOfDay.Night:
                    OnNight?.Invoke();
                    break;
            }
        }
    }
}
