using RPG.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [SerializeField] private List<Objective> _objectives = new List<Objective>();
        [SerializeField] private List<Reward> _rewards = new List<Reward>();

        public IEnumerable<Objective> Objectives => _objectives;
        public IEnumerable<Reward> Rewards => _rewards;

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return _objectives.Count;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (var objective in _objectives)
            {
                if (objective.reference == objectiveRef)
                {
                    return true;
                }
            }

            return false;
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }

            return null;
        }
    }
}
