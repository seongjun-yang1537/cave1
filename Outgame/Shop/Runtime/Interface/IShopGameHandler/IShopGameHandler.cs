using UnityEngine.Events;

namespace Outgame
{
    public interface IShopGameHandler
    {
        public int Day { get; }
        public UnityEvent<int> OnNextDay { get; }
    }
}