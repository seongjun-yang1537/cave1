using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class ViewDragonBoarBodyAnimation : ViewBehaviour<DragonBoarView>
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