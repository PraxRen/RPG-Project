using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private Experience _experience;

        private void Start()
        {
            _experience = PersistentObjects.Instance.Player.GetComponent<Experience>();
        }

        private void Update()
        {
            _text.text = string.Format("{0:0}", _experience.GetPoints());
        }
    }
}

