using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(AntScope))]
    public class AntView : MonsterView
    {
        [AutoSubscribe(nameof(onAnimationStartAttack))]
        protected virtual void OnAnimationStartAttack(AgentController attacker, AgentController target)
        {
            SetAnimationTrigger("Attack");
        }
    }
}