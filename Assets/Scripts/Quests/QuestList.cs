using RPG.Inventories;
using RPG.Saving;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [RequireComponent(typeof(Inventory))]
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        private List<QuestStatus> _statuses = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestStatuses => _statuses;

        public event Action OnUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest))
                return;

            QuestStatus newStatus = new QuestStatus(quest);
            _statuses.Add(newStatus);
            OnUpdate?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);

            if (status.IsComplete())
            {
                GiveReward(quest);
            }

            OnUpdate?.Invoke();
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach (QuestStatus status in _statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;

            if (stateList == null)
                return;

            _statuses.Clear();

            foreach (object objectState in stateList)
            {
                _statuses.Add(new QuestStatus(objectState));
            }
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in _statuses)
            {
                if (status.Quest == quest)
                {
                    return status;
                }
            }

            return null;
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.Rewards)
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);

                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        private bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetByName(parameters[0]));
                case "CompletedQuest":
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            }

            return null;
        }
    }
}