using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIEquipmentSlotIcon : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private Image imgHelmet;
        [Required, ReferenceBind, SerializeField] private Image imgChestplate;
        [Required, ReferenceBind, SerializeField] private Image imgLeggings;
        [Required, ReferenceBind, SerializeField] private Image imgBoots;
        [Required, ReferenceBind, SerializeField] private Image imgJetpack;
        [Required, ReferenceBind, SerializeField] private Image imgOxygenTank;
        [Required, ReferenceBind, SerializeField] private Image imgBag;

        [Group("Placeholder")] public EquipmentType equipmentType;

        [Button("Render")]
        public override void Render()
        {
            SetActiveAll(false);
            switch (equipmentType)
            {
                case EquipmentType.Helmet: imgHelmet.gameObject.SetActive(true); break;
                case EquipmentType.Chestplate: imgChestplate.gameObject.SetActive(true); break;
                case EquipmentType.Leggings: imgLeggings.gameObject.SetActive(true); break;
                case EquipmentType.Boots: imgBoots.gameObject.SetActive(true); break;
                case EquipmentType.Jetpack: imgJetpack.gameObject.SetActive(true); break;
                case EquipmentType.OxygenTank: imgOxygenTank.gameObject.SetActive(true); break;
                case EquipmentType.Bag: imgBag.gameObject.SetActive(true); break;
            }
        }

        public void Render(EquipmentType equipmentType)
        {
            this.equipmentType = equipmentType;
            Render();
        }

        void SetActiveAll(bool value)
        {
            imgHelmet.gameObject.SetActive(value);
            imgChestplate.gameObject.SetActive(value);
            imgLeggings.gameObject.SetActive(value);
            imgBoots.gameObject.SetActive(value);
            imgJetpack.gameObject.SetActive(value);
            imgOxygenTank.gameObject.SetActive(value);
            imgBag.gameObject.SetActive(value);
        }
    }
}
