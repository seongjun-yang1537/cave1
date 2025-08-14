using UnityEngine;

namespace UI
{
    public interface IContextUIRenderer
    {
        GameObject gameObject { get; }
        void Render(ContextUIContext context);
        void Show();
        void Hide();
    }
}
