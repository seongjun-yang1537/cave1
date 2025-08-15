// 파일: ItemTooltipModel.cs
using Ingame;
using UnityEngine;

namespace UI
{

    public class ItemTooltipModel : TooltipUIModel
    {
        public ItemModel itemModel;
        public int sellPrice = -1;

        public ItemTooltipModel(UIMonoBehaviour bindUI) : base(bindUI)
        {
        }
    }
}