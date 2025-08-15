using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIItemSlotElement : UIMonoBehaviour,
        IPointerDownHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        public UnityEvent onClick = new();
        public UnityEvent<PointerEventData> onDragStart = new();
        public UnityEvent<PointerEventData> onDrag = new();
        public UnityEvent<PointerEventData> onDragEnd = new();

        [Group("Placeholder")]
        public InventorySlotModel itemSlot;
        public ItemModel itemModel => itemSlot?.itemModel;

        private ItemTooltipUIModel TooltipUIModel;

        protected override void Awake()
        {
            base.Awake();
            TooltipUIModel = new(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            itemSlot = null;
            Render();
        }

        public override void Render()
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onDragStart.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onDragEnd.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemModel == null || itemModel.IsEmpty)
                return;

            TooltipUIModel.itemModel = itemModel;
            TooltipUISystem.Show(TooltipUIModel);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (itemModel == null || itemModel.IsEmpty)
                return;

            TooltipUISystem.Hide(null);
        }
    }
}