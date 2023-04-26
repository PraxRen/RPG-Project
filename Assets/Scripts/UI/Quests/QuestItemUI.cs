using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        public QuestStatus QuestStatus { get; private set; }

        public void Setup(QuestStatus status)
        {
            QuestStatus = status;
            title.text = status.Quest.GetTitle();
            progress.text = status.GetCompletedCount() + "/" + status.Quest.GetObjectiveCount();
        }
    }
}