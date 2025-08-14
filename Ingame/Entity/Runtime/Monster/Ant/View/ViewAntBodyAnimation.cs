using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class ViewAntBodyAnimation : ViewBehaviour<AntView>
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