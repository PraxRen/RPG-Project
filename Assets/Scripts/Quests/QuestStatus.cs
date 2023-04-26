using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }

        private Quest _quest;
        private List<string> _completedObjectives =  new List<string>();

        public Quest Quest => _quest;

        public QuestStatus(Quest quest)
        {
            _quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            _quest = Quest.GetByName(state.questName);
            _completedObjectives = state.completedObjectives;
        }

        public int GetCompletedCount()
        {
            return _completedObjectives.Count;
        }

        public bool IsComplete()
        {
            foreach (var objective in Quest.Objectives)
            {
                if (!_completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return _completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (!_quest.HasObjective(objective))
                return;
            
            _completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = _quest.name;
            state.completedObjectives = _completedObjectives;
            return state;
        }
    }
}