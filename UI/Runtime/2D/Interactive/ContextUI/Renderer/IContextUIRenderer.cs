using UnityEngine;

namespace UI
{
    public interface IContextUIRenderer
    {
        GameObject gameObject { get; }
        void Render(ContextUIModel context);
        void Show();
        void Hide();
    }
}
