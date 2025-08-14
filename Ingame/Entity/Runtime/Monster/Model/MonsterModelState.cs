using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class MonsterModelState : PawnModelState
    {
        public float sightRange = 10f;
        public float attackRange = 0f;
        public float attackCooldown = 2.0f;
        public ItemDropTable dropTable;
        public override Type TargetType => typeof(MonsterModel);
    }
}
