using System;

namespace Ingame
{
    public interface IStatusEffect
    {
        public void OnApply(AgentController agentController);
        public void OnRemove(AgentController agentController);
        public bool IsExpired();
    }
}