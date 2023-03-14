using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.AI;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] _cursorMappings = null;
        [SerializeField] private float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float _maxNavPathLength = 40f;

        private Mover _mover;
        private Fighter _fighter;
        private Health _health;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI())
                return;

            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent())
                return;
            
            if (InteractWithMovement())
                return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits) 
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }


        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }

            return _cursorMappings[0];
        }

        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavMesh(out Vector3 target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = Vector3.zero;
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);

            if (!hasHit)
                return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh)
                return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if (!hasPath)
                return false;

            if (path.status != NavMeshPathStatus.PathComplete)
                return false;

            if (GetPathLength(path) > _maxNavPathLength)
                return false;

            return hasCastToNavMesh;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;

            if (path.corners.Length < 2)
                return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

