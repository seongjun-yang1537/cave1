using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIDraggingItem : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private Image imgIcon;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtCount;

        [Group("Placeholder")]
        public ItemModel itemModel;

        protected void Start()
        {
            Render(null);
        }

        [Button("Render")]
        public override void Render()
        {
            bool active = itemModel != null && !itemModel.IsEmpty;
            imgIcon.gameObject.SetActive(active);
            txtCount.gameObject.SetActive(active);

            if (itemModel != null)
            {
                imgIcon.sprite = ItemDB.GetIconSprite(itemModel.itemID);
                txtCount.text = $"{itemModel.count}";
                txtCount.gameObject.SetActive(itemModel.count > 1);
            }
        }

        public void Render(ItemModel itemModel)
        {
            this.itemModel = itemModel;
            Render();
        }
    }
}