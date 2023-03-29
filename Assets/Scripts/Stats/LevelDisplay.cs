using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private BaseStats _baseStats;
        [SerializeField] private Text _text;

        private void Update()
        {
            _text.text = string.Format("{0:0}", _baseStats.GetLevel());
        }
    }
}

