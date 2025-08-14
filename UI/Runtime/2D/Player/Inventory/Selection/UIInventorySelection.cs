using Ingame;
using UnityEngine;
using DG.Tweening;
using TriInspector;

namespace UI
{

    public class UIInventorySelection : UIMonoBehaviour
    {
        public float moveTimer = 0.1f;

        [Group("Placeholder")]
        public UIItemSlotElement uiItemSlotElement;

        [Button("Render")]
        public override void Render()
        {
            if (uiItemSlotElement == null)
                return;

            transform.DOMove(uiItemSlotElement.transform.position, moveTimer);
        }

        public void Render(UIItemSlotElement uiItemSlotElement)
        {
            this.uiItemSlotElement = uiItemSlotElement;
            Render();
        }
    }
}