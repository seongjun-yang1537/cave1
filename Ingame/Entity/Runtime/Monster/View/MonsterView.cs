using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(MonsterScope))]
    public class MonsterView : PawnView
    {
        protected override void Update()
        {
            base.Update();
            SetAnimationFloat("MoveMagnitude", Mathf.Approximately(deltaPosition.magnitude, 0f) ? 0 : 1);
        }
    }
}