using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventory _inventoryUI;

        [SerializeField] private InventorySO _inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Start()
        {
            PrepareUI();
            PrepateInventoryData();

            foreach (var item in _inventoryData.GetCurrentInventoryState())
            {
                _inventoryUI.UpdateData(item.Key, item.Value.item.ItemSprite, item.Value.quantity);
            }
        }

        private void PrepateInventoryData()
        {
            _inventoryData.Init();
            _inventoryData.OnInventoryChanged += UpdateInventoryUI;

            foreach(var item in initialItems)
            {
                if (item.IsEmpty)
                    continue;

                _inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            _inventoryUI.ResetAllItems();

            foreach (var item in inventoryState)
            {
                _inventoryUI.UpdateData(item.Key, item.Value.item.ItemSprite, item.Value.quantity);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (_inventoryUI.isActiveAndEnabled == false)
                {
                    _inventoryUI.Show();

                    foreach (var item in _inventoryData.GetCurrentInventoryState())
                    {
                        _inventoryUI.UpdateData(item.Key, item.Value.item.ItemSprite, item.Value.quantity);
                    }
                }
                else
                {
                    _inventoryUI.Hide();
                }
            }
        }

        private void PrepareUI()
        {
            _inventoryUI.InitInventoryUI(_inventoryData.Size);
            _inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
            _inventoryUI.OnItemActionRequested += HandleItemActionRequested;
            _inventoryUI.OnStartDragging += HandleStartDragging;
            _inventoryUI.OnSwapItems += HandleSwapItems;
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            _inventoryData.SwapItem(itemIndex_1, itemIndex_2);
        }

        private void HandleStartDragging(int itemIndex)
        {
            InventoryItem item = _inventoryData.GetItemAt(itemIndex);
            
            if(item.IsEmpty)
                return;

            _inventoryUI.CreateDraggedItem(item.item.ItemSprite, item.quantity);
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            InventoryItem item = _inventoryData.GetItemAt(itemIndex);

            if(item.IsEmpty) 
                return;

            IDestroyableItem destroyableItem = item.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                _inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = item.item as IItemAction;          
            if(itemAction != null)
            {
                itemAction.PerformAction(gameObject, item.itemState);
            }
        }

        private void HandleDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
                return;
            }
                
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            _inventoryUI.UpdateDescription(itemIndex, item.ItemSprite,item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();

            for(int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

