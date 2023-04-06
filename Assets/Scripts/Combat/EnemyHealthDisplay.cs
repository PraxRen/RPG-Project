using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private Fighter _fighter;

        private void Start()
        {
            _fighter = PersistentObjects.Instance.Player.GetComponent<Fighter>();
        }

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
