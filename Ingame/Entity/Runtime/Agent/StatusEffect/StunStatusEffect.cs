using UnityEngine;

namespace Ingame
{
    public class StunStatusEffect : IStatusEffect
    {
        public float durationSeconds = 3.0f;
        private float applyTime;

        public bool IsExpired()
            => Time.time - applyTime >= durationSeconds;

        public void OnApply(AgentController agentController)
        {
            applyTime = Time.time;
        }

        public void OnRemove(AgentController agentController)
        {

        }
    }
}