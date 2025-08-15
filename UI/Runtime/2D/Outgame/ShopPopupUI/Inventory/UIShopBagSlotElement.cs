using Codice.CM.Common;
using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIShopBagSlotElement : UIBagSlotElement
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    var contextModel = new ShopSellContextUIModel()
                    {
                        slotModel = itemSlot,
                        price = 1000,
                    };
                    ContextUISystem.Show(contextModel);
                    break;
            }
        }
    }
}