using UnityEngine;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UIInventoryItem _item;

        private void Awake()
        {
            _canvas = transform.root.GetComponent<Canvas>();
            _item = GetComponentInChildren<UIInventoryItem>();
        }

        private void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_canvas.transform,
                Input.mousePosition,
                _canvas.worldCamera,
                out position
                );

            transform.position = _canvas.transform.TransformPoint(position);
        }

        public void SetData(Sprite sprite, int quantity)
        {
            _item.SetData(sprite, quantity);
        }

        public void Toggle(bool val)
        {
            Debug.Log($"Item toggle: {val}");
            gameObject.SetActive(val);
        }

    }
}

