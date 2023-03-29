using RPG.Attributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Text _text;

        private void Update()
        {
            if ( _fighter.GetTarget() == null ) 
            {
                _text.text = "N/A";
                return;
            }

            Health health = _fighter.GetTarget();
            _text.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
