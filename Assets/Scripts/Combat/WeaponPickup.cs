using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weapon = null;
        [SerializeField] private float _healthToRestore = 0;
        [SerializeField] private float _respawnTime = 5;

        public bool HandleRaycast(PlayerController callingController)
        {
            Mover mover = callingController.GetComponent<Mover>();

            if (Input.GetMouseButton(0) && mover.CanMoveTo(transform.position))
            {
                mover.StartMoveAction(transform.position, 1f);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.ToLower() == "player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (_weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weapon);
            }

            if (_healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(_healthToRestore);
            }
           
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        private IEnumerator HideForSeconds(float respawnTime)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(respawnTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
