using UnityEngine;
using System;
using UnityEngine.Events;
using Ingame.ModelDataSheet;
using UnityEditorInternal;

namespace Ingame
{
    [Serializable]
    public class OBJDeliveryBoxModel : EntityModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== State ==========
        public ItemModel itemModel;
        #endregion ====================

        public new OBJDeliveryBoxModelData Data => base.Data as OBJDeliveryBoxModelData;

        public OBJDeliveryBoxModel(OBJDeliveryBoxModelData data, OBJDeliveryBoxModelState state = null) : base(data, state)
        {
            if (state != null)
            {
                ItemModelData itemModelData = ItemDB.GetItemModelData(state.itemModelState.itemID);
                this.itemModel = ItemModelFactory.Create(itemModelData, state.itemModelState);
            }
        }
    }
}