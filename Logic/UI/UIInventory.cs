using System;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory.UI
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem _itemPrefab;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private UIInventoryDescription _descriptionPanel;
        [SerializeField] private MouseFollower _mouseFollower;

        List<UIInventoryItem> listUIItems = new List<UIInventoryItem>();

        private int _currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            Show();
        }

        public void InitInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_contentPanel);
                listUIItems.Add(item);

                item.OnItemClicked += HandleItemSelection;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnRightMouseButtonClick += HandleShowItemActions;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (listUIItems.Count > itemIndex)
            {
                listUIItems[itemIndex].SetData(itemSprite, itemQuantity);
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            var index = listUIItems.IndexOf(item);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            var index = listUIItems.IndexOf(item);
            if (index == -1)
                return;

            _currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(index);

        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();

        }

        private void HandleSwap(UIInventoryItem item)
        {
            var index = listUIItems.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(_currentlyDraggedItemIndex, index);
            HandleItemSelection(item);
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            var index = listUIItems.IndexOf(item);
            if(index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void ResetDraggedItem()
        {
            _mouseFollower.Toggle(false);
            _currentlyDraggedItemIndex = -1;
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            _mouseFollower.Toggle(true);
            _mouseFollower.SetData(sprite, quantity);
        }

        public void ResetSelection()
        {
            _descriptionPanel.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (var item in listUIItems)
            {
                item.Deselect();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);

            ResetSelection();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemSprite, string name, string description)
        {
            _descriptionPanel.SetDescription(itemSprite, name, description);
            DeselectAllItems();
            listUIItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach(var item in listUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}

