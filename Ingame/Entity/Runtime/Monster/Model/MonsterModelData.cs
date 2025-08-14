using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class MonsterModelData : PawnModelData
    {
        public override Type TargetType => typeof(MonsterModel);
        public float sightRange = 10f;
        public float attackRange;
        public float attackCooldown = 2.0f;
        public ItemDropTable dropTable;

        public float enemyDetectionRange = 100f;
    }
}

