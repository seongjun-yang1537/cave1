// 파일: ITooltipRenderer.cs
using UnityEngine;

namespace UI
{
    public interface ITooltipRenderer
    {
        GameObject gameObject { get; }
        void Render(TooltipUIModel context);
        void Show();
        void Hide();
    }
}