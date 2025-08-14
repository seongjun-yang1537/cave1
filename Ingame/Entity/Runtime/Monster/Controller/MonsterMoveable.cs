using UnityEngine;
using VContainer.Unity;

namespace Ingame
{
    public class MonsterMoveable : PawnMoveable, IInitializable
    {
        protected MonsterModel monsterModel;

        public override void Initialize()
        {
            base.Initialize();
            monsterModel = (MonsterModel)pawnModel;
        }

        public override void Update()
        {
            base.Update();
        }

        public override Quaternion? GetNextRotation()
        {
            Vector3 forward = directionProvider.Forward;
            forward.y = 0f;
            forward.Normalize();
            if (forward.sqrMagnitude < 0.01f) return null;
            return Quaternion.LookRotation(forward);
        }
    }
}