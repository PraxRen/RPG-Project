using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Transform _objectiveContainer;
        [SerializeField] private GameObject _objectivePrefab;
        [SerializeField] private GameObject _objectiveIncompletePrefab;
        [SerializeField] private TextMeshProUGUI _rewardText;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.Quest;
            _title.text = quest.GetTitle();

            foreach (Transform child in _objectiveContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Quest.Objective objective in quest.Objectives)
            {
                GameObject prefab = _objectiveIncompletePrefab;

                if (status.IsObjectiveComplete(objective.reference))
                {
                    prefab = _objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate(prefab, _objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }

            _rewardText.text = GetRewardText(quest);
        }

        private string GetRewardText(Quest quest)
        {
            StringBuilder rewardText = new StringBuilder();

            foreach (var reward in quest.Rewards)
            {
                if (rewardText.Length > 0)
                {
                    rewardText.Append(", ");
                }

                if (reward.number > 1)
                {
                    rewardText.Append(reward.number + " ");
                }

                rewardText.Append(reward.item.GetDisplayName());
            }

            if (rewardText.Length == 0)
            {
                rewardText.Append("No reward");
            }

            rewardText.Append(".");
            return rewardText.ToString();
        }
    }
}
