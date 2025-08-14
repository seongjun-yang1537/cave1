using UnityEngine.Events;

namespace UI
{
    public interface IPopupUIController
    {
        UnityEvent<bool> onVisible { get; }
        bool visible { get; }
        bool allowMultipleInstances { get; }
        void Open();
        void Close();
        void SetVisible(bool newVisible);
    }
}
