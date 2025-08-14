using System;
using System.Collections.Generic;
using Corelib.Utils;
using Ingame;
using System.Linq;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(ToomanyItemUIView))]
    public class ToomanyItemUIController : UIControllerBaseBehaviour<ToomanyItemUIView>
    {
        private InventoryModel inventory;
        private static bool visible;

        protected override void Awake()
        {
            base.Awake();
            SetVisible(visible);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.BackQuote)) ToggleVisible();
        }

        public void Bind(InventoryModel inventory)
        {
            this.inventory = inventory;
        }

        private void GiveItem(ItemID itemID, int count)
        {
            inventory?.AddItem(ItemModelFactory.Create(new ItemModelState { itemID = itemID, count = count }));
        }

        private void ToggleVisible() => SetVisible(!visible);

        private void SetVisible(bool newVisible)
        {
            visible = newVisible;
            view.onVisible.Invoke(newVisible, GiveItem);
        }
    }
}
