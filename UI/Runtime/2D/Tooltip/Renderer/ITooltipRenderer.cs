// 파일: ITooltipRenderer.cs
using UnityEngine;

namespace UI
{
    public interface ITooltipRenderer
    {
        GameObject gameObject { get; }
        void Render(TooltipContext context);
        void Show();
        void Hide();
    }
}