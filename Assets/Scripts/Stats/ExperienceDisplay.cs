using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Experience _experience;
        [SerializeField] private Text _text;

        private void Update()
        {
            _text.text = string.Format("{0:0}", _experience.GetPoints());
        }
    }
}

