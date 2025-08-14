namespace Ingame
{
    public interface IAgentAttackable
    {
        public float Attack(AgentController attacker, AgentController target, AttackContext info);
    }
}