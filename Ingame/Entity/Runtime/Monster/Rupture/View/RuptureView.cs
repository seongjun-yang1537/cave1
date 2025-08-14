using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(RuptureScope))]
    public class RuptureView : MonsterView
    {
        [AutoSubscribe(nameof(onAttack))]
        protected virtual void OnAttack(AgentController attacker, AgentController target, float damage)
        {
            SetAnimationTrigger("Attack");
        }
    }
}