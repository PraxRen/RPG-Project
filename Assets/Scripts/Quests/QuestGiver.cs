using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveQuest()
        {
            QuestList questList = PersistentObjects.Instance.Player.GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}
