using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints = 0;

        public event Action OnExperienceGained;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained();
        }

        public float GetPoints()
        {
            return _experiencePoints;
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}
