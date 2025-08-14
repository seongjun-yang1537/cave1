using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DeclareBoxGroup("Tooltip")]
    public class UIItemTooltipBase : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField, Group("Tooltip")]
        protected Image imgIcon;

        [Group("Placeholder"), SerializeField]
        protected ItemModel itemModel;

        public override void Render()
        {

        }

        public void Render(ItemModel itemModel)
        {
            this.itemModel = itemModel;
            Render();
        }
    }
}