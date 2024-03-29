using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;

        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;
        private AIConversant _currentConversant = null;
        private bool _isChoosing;

        public event Action OnConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            _currentConversant = newConversant;
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public void Quit()
        {
            _currentDialogue = null;
            TriggerExitAction();
            _currentNode = null;
            _isChoosing = false;
            _currentConversant = null;
            OnConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return _isChoosing;
        }

        public string GetText()
        {
            if (_currentNode == null)
                return "";

            return _currentNode.Text;
        }

        public string GetCurrentConversantName()
        {
            if (_isChoosing)
            {
                return playerName;
            }

            return _currentConversant.Name;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            TriggerEnterAction();
            _isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode)).Count();

            if (numPlayerResponses > 0)
            {
                _isChoosing = true;
                TriggerExitAction();
                OnConversationUpdated?.Invoke();
                return;
            }

            DialogueNode[] children = FilterOnCondition(_currentDialogue.GetAlChildren(_currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitAction();
            _currentNode = children[randomIndex];
            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return FilterOnCondition(_currentDialogue.GetAllChildren(_currentNode)).Count() > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if (_currentNode != null && !string.IsNullOrEmpty(_currentNode.OnEnterAction))
            {
                TriggerAction(_currentNode.OnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (_currentNode != null && !string.IsNullOrEmpty(_currentNode.OnExitAction))
            {
                TriggerAction(_currentNode.OnExitAction);
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "")
                return;

            foreach (DialogueTrigger trigger in _currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }
}
