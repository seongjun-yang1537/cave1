namespace Ingame
{
    public class DefaultAgentAttackable : IAgentAttackable
    {
        public float Attack(AgentController attacker, AgentController target, AttackContext info)
        {
            AgentModel attackerMoel = attacker.agentModel;
            float damage = info.damage;

            if (target != null)
            {
                damage = attackerMoel.CalculateDamage(target.agentModel, damage);
                damage = target.TakeDamage(attackerMoel, damage);
            }

            return damage;
        }
    }
}