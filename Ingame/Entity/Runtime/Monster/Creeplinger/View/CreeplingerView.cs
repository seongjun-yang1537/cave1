using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(CreeplingerScope))]
    public class CreeplingerView : MonsterView
    {
        [AutoSubscribe(nameof(onAnimationStartAttack))]
        protected virtual void OnAnimationStartAttack(AgentController attacker, AgentController target)
        {
            SetAnimationTrigger("Attack");
        }
    }
}