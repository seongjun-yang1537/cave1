using Codice.CM.Common;
using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIShopQuickSlotElement : UIQuickSlotElement
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    var contextModel = new ShopSellContextUIModel()
                    {
                        count = 1,
                        price = 1000,
                    };
                    ContextUISystem.Show(contextModel);
                    break;
            }
        }
    }
}