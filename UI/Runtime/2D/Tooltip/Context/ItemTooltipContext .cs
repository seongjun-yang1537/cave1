// 파일: ItemTooltipContext.cs
using Ingame;
using UnityEngine;

namespace UI
{

    public class ItemTooltipContext : TooltipContext
    {
        public ItemModel itemModel;
        public int sellPrice = -1;
    }
}