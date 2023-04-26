using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private Quest _quest;
        [SerializeField] private string _objective;

        public void CompleteObjective()
        {
            QuestList questList = PersistentObjects.Instance.Player.GetComponent<QuestList>();
            questList.CompleteObjective(_quest, _objective);
        }
    }
}