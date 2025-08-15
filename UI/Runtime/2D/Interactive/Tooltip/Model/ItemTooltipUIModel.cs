// 파일: ItemTooltipUIModel.cs
using Ingame;
using UnityEngine;

namespace UI
{

    public class ItemTooltipUIModel : TooltipUIModel
    {
        public ItemModel itemModel;
        public int sellPrice = -1;

        public ItemTooltipUIModel(UIMonoBehaviour bindUI) : base(bindUI)
        {
        }
    }
}