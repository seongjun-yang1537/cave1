using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIEquipmentSlotElement : UIItemSlotElement
    {
        [Required, ReferenceBind, SerializeField] private UIEquipmentSlotFrame uiSlotFrame;
        [Required, ReferenceBind, SerializeField] private UIEquipmentSlotIcon uiSlotIcon;
        [Required, ReferenceBind, SerializeField] private Image imgItem;

        [Group("Placeholder")]
        public EquipmentType equipmentType;

        public override void Render()
        {
            bool active = itemSlot != null && !itemSlot.IsEmpty;
            imgItem.gameObject.SetActive(active);

            if (!active) return;
            switch (equipmentType)
            {
                case EquipmentType.Helmet:
                case EquipmentType.Chestplate:
                    uiSlotFrame.Render("red");
                    break;

                case EquipmentType.Leggings:
                case EquipmentType.Boots:
                    uiSlotFrame.Render("blue");
                    break;

                case EquipmentType.Jetpack:
                case EquipmentType.OxygenTank:
                case EquipmentType.Bag:
                    uiSlotFrame.Render("gold");
                    break;
            }

            uiSlotIcon.Render(equipmentType);
            imgItem.sprite = ItemDB.GetIconSprite(itemSlot.itemModel.itemID);
        }

        public void Render(InventorySlotModel itemSlot)
        {
            if (itemSlot == null)
            {
                imgItem.gameObject.SetActive(false);
                return;
            }

            this.itemSlot = itemSlot;
            ItemID itemID = itemSlot.itemModel.itemID;
            Render(itemID.GetEquipmentType());
        }

        public void Render(EquipmentType equipmentType)
        {
            this.equipmentType = equipmentType;
            Render();
        }
    }
}