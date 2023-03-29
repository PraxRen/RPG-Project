using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Text _text;

        private void Update()
        {
            _text.text = string.Format("{0:0}/{1:0}", _health.GetHealthPoints(), _health.GetMaxHealthPoints());
        }
    }
}
