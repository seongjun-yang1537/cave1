using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UI
{
    public class UIToomanyItemElement : UIMonoBehaviour, IPointerClickHandler
    {
        public UnityEvent<PointerEventData> onPointerClick = new();

        [Required, ReferenceBind, SerializeField] private Image imgIcon;

        [Group("Placeholder"), SerializeField]
        private ItemID itemID;

        public override void Render()
        {
            imgIcon.sprite = ItemDB.GetIconSprite(itemID);
        }

        public void Render(ItemID itemID)
        {
            this.itemID = itemID;
            Render();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick.Invoke(eventData);
        }
    }
}
