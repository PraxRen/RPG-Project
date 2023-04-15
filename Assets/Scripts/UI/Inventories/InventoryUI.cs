using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;
using RPG.Core;

namespace RPG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;

        private Inventory _playerInventory;

        private void Start() 
        {
            var player = PersistentObjects.Instance.Player;
            _playerInventory = player.GetComponent<Inventory>();
            _playerInventory.inventoryUpdated += Redraw;
            Redraw();
        }

        private void OnDestroy()
        {
            if (_playerInventory != null)
            {
                _playerInventory.inventoryUpdated -= Redraw;
            }
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(_playerInventory, i);
            }
        }
    }
}