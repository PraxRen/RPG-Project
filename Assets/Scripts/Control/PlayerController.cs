using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Health _health;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead())
                return;

            if (InteractWithCombat())
                return;
            
            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();

                if (target == null)
                    continue;

                if (!_fighter.CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                    
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            bool isHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);

            if (isHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point, 1f);
                }

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

