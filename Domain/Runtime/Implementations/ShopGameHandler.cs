using Outgame;
using UnityEngine.Events;

namespace Domain
{
    public class ShopGameHandler : IShopGameHandler
    {
        public int Day => GameController.Time.Day;
        public UnityEvent<int> OnNextDay => GameController.Time.OnNextDay;
    }
}