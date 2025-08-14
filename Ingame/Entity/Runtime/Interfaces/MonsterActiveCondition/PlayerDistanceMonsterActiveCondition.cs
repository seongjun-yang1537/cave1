using UnityEngine;
using VContainer;

namespace Ingame
{
    public class PlayerDistanceMonsterActiveCondition : IMonsterActiveCondition
    {
        [Inject] private readonly Transform transform;
        [Inject] private readonly MonsterModel monsterModel;

        private float MaxDistance => monsterModel.sightRange * 3f;

        public bool IsActive()
        {
            var player = PlayerSystem.CurrentPlayer;
            if (player == null)
                return true;
            float sqr = (transform.position - player.transform.position).sqrMagnitude;
            return sqr <= MaxDistance * MaxDistance;
        }
    }
}
