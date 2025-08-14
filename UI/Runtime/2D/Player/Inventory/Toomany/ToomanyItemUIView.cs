using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ToomanyItemUIView : UIViewBaseBehaviour
    {
        public UnityEvent<bool, UnityAction<ItemID, int>> onVisible = new();

        [SerializeField]
        private CanvasGroup canvasGroup;

        [Required, ReferenceBind, SerializeField]
        private UIToomanyItemList uiItemList;

        [Group("Placeholder")]
        public List<ItemModel> items;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [AutoSubscribe(nameof(onVisible))]
        private void OnVisible(bool visible, UnityAction<ItemID, int> onGiveItem)
        {
            if (canvasGroup != null) canvasGroup.alpha = visible ? 1f : 0f;

            List<ItemID> itemIDs = Enum.GetValues(typeof(ItemID))
                .Cast<ItemID>()
                .Where(itemID => itemID != ItemID.None)
                .ToList();

            uiItemList.Render(itemIDs);
            uiItemList.onItemClick.RemoveAllListeners();
            uiItemList.onItemClick.AddListener(onGiveItem);
        }
    }
}
