using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Codice.CM.WorkspaceServer.Lock;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIInventoryEquipment : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private UIEquipmentSlotList uiEquipmentSlotList;
        [Required, ReferenceBind, SerializeField] private UIPlayerProfile uiPlayerProfile;
        [Required, ReferenceBind, SerializeField] private UIPlayerStat uiPlayerStat;

        [Group("Placeholder"), SerializeField]
        private PlayerModel playerModel;

        public void UpdateEquipmentSlot(InventorySlotModel itemSlot)
        {
            uiEquipmentSlotList.UpdateSlot(itemSlot);
        }

        public override void Render()
        {
            EquipmentContainer equipmentContainer = playerModel.inventory.equipmentContainer;

            List<InventorySlotModel> itemSlots = equipmentContainer.ToList();
            uiEquipmentSlotList.Render(itemSlots);

            uiPlayerProfile.Render(playerModel);
            uiPlayerStat.Render(playerModel);
        }

        public void Render(PlayerModel playerModel)
        {
            this.playerModel = playerModel;
            Render();
        }
    }
}