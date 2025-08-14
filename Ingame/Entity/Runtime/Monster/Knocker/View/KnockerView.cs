using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(KnockerScope))]
    public class KnockerView : MonsterView
    {
        [AutoSubscribe(nameof(onAnimationStartAttack))]
        protected virtual void OnAnimationStartAttack(AgentController attacker, AgentController target)
        {
            SetAnimationTrigger("Attack");
        }
    }
}