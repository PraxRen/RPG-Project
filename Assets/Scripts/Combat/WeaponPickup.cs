using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weapon = null;
        [SerializeField] private float _healthToRestore = 0;
        [SerializeField] private float _respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
