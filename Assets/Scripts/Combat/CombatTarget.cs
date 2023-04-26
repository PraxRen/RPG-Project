using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled)
            {
                return false;
            }

            if (callingController.TryGetComponent(out Fighter fighter) == false)
            {
                return false;
            }

            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }

            return true;
        }
    }
}