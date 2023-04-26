using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private QuestItemUI _questPrefab;
        private QuestList _questList;

        private void Start()
        {
            _questList = PersistentObjects.Instance.Player.GetComponent<QuestList>();
            _questList.OnUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus status in _questList.QuestStatuses)
            {
                QuestItemUI uiInstance = Instantiate<QuestItemUI>(_questPrefab, transform);
                uiInstance.Setup(status);
            }
        }
    }
}
