using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(DragonBoarScope))]
    public class DragonBoarView : MonsterView
    {
        [AutoSubscribe(nameof(onAnimationStartAttack))]
        protected virtual void OnAnimationStartAttack(AgentController attacker, AgentController target)
        {
            SetAnimationTrigger("Attack");
        }
    }
}