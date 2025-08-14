using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class ViewCreeplingerBodyAnimation : ViewBehaviour<CreeplingerView>
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