using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private BaseStats _baseStats;

        private void Start()
        {
            _baseStats = PersistentObjects.Instance.Player.GetComponent<BaseStats>();
        }
        private void Update()
        {
            _text.text = string.Format("{0:0}", _baseStats.GetLevel());
        }
    }
}

