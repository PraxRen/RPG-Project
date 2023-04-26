using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private string _conversantName;

        public string Name => _conversantName;

        private void Start()
        {
            if (string.IsNullOrEmpty(_conversantName))
            {
                _conversantName = gameObject.name;
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (_dialogue == null)
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, _dialogue);
            }

            return true;
        }
    }
}
