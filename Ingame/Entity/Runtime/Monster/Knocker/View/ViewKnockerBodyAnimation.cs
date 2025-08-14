using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class ViewKnockerBodyAnimation : ViewBehaviour<KnockerView>
    {
        public void OnAttackEnd()
        {
            rootView.onAnimationEndAttack.Invoke();
        }

        public void Render()
        {

        }
    }
}