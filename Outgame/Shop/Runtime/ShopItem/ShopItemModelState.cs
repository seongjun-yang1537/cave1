using System;
using Ingame;

namespace Outgame
{
    [Serializable]
    public class ShopItemModelState
    {
        public ItemModelState itemModel;
        public ShopItemPhase phase;
        public int remainDeliverDays;
    }
}